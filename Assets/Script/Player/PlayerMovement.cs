using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    #region Properties
    public Vector2 CurrentVelocity => rb2d.linearVelocity;
    public Vector2 TargetVelocity => targetVelocity;
    public Vector2 WallNormal => wallNormal;
    public Vector2 InputDirection => inputDirection;
    public Vector2 GroundNormal => groundNormal;
    
    public bool IsGrounded => isGrounded;
    public bool IsClimbing => isClimbing;
    public bool IsWalled => isWalled;
    public bool IsRolling => isWalled;
    public Rigidbody2D Rb2d => rb2d;
    #endregion
    
    private PlayerAnimatorController animatorController;
    private PlayerInput playerInput;
    private Rigidbody2D rb2d;
    private Player player;



    [Header("Movement")]
    [SerializeField]
    private GameObject normalCollider;
    [SerializeField] 
    private float gravityScale = 8;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float airAcceleration;
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float airDeceleration;

    [SerializeField] 
    private float stopForce;
    [SerializeField, Range(1, 4)] 
    private float semiTurnForceMultiplier;
    
    [SerializeField]
    private int maxSpeed = 10;
    [SerializeField]
    private float jumpForce;
    [SerializeField] 
    private float maxFallSpeed = 10;
    
    [Header("Ground check")]
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField, Range(0f, 1f)]
    private float groundCheckRadius;
    [SerializeField]
    private float minGroundAngle;
    [SerializeField]
    private float maxGroundAngle;
    
    [Header("Wall check")]
    [SerializeField]
    private LayerMask wallLayer;
    [SerializeField, Range(0f, 1f)]
    private float wallCheckRadius;
    [SerializeField]
    private float wallAngle;
    [SerializeField]
    private float wallNormalJumpForce;
    [SerializeField, Range(0f, 1f)]
    private float wallJumpForceMultiplier;
    [SerializeField, Range(0f, 1f)]
    private float wallFallSpeedMultiplier;
    [SerializeField, Range(0f, 2f)]
    private float wallJumpSpeedMultiplier;
    
    [Header("Climb")]
    [SerializeField]
    private float climbForce;
    [SerializeField]
    private float climbMaxSpeed;
    [SerializeField]
    private float climbAcceleration;
    [SerializeField]
    private float climbDeceleration;
    [SerializeField, Range(1f, 100f)] 
    private float climbFallSpeed;
    
    [Header("Glide check")]
    [SerializeField, Range(0f, 1f)] 
    private float glidingFallSpeedMultiplier;

    [Header("Rolling check")] 
    [SerializeField]
    private GameObject rollCollider;
    [SerializeField]
    private float initialRollSpeedModifier;
    [SerializeField] 
    private float rollingForce;
    [SerializeField] 
    private float minRollTime;
    [SerializeField] 
    private float rollHeightCheck;
    [SerializeField] 
    private float stopRollTime;
    [SerializeField] 
    private AnimationCurve stopRollCurve;
    
    [Header("Falling check")]
    [SerializeField]
    private int fallTimerSeconds;
    [SerializeField]
    private float fallTimerMultiplier;
    
    private float fallTimerMultiplierInGliding = 1;
    private float fallBackTimer;
    private bool canStartFallTimer = true;
    
    private bool isGrounded;
    private bool isJumping;
    private bool isWalled;
    private bool isClimbing;
    private bool isWantsToGlide;
    private bool isRolling;
    
    private int wantsToJump;
    private int wantsToRoll;
    
    private Camera playerCamera;

    private Vector2 groundNormal;
    private Vector2 wallNormal;

    private Vector2 targetVelocity;
    private Vector2 inputDirection;
    private Vector2 wallCheckDirection;
    private Vector2 lastPosOnGround;
    
    private RaycastHit2D[] hits;

    private int frameCounter;
    

    private void OnValidate()
    {
        GetComponent<Rigidbody2D>().gravityScale = gravityScale;
    }

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rb2d = GetComponent<Rigidbody2D>();
        playerCamera = GetComponentInChildren<Camera>();
        animatorController = GetComponent<PlayerAnimatorController>();
        hits = new RaycastHit2D[32];
        fallBackTimer = fallTimerSeconds;
    }

    private void FixedUpdate()
    {
        frameCounter++;
        if(wantsToJump > 0)
            wantsToJump--;
        
        if(wantsToRoll > 0)
            wantsToRoll--;
        if (isGrounded)
        {
            isJumping = false;
            canStartFallTimer = true;
        }
        
        if (isWalled)
            canStartFallTimer = true;
        HandleGround();
        HandleWalls();

        if (!isGrounded)
        {
            if (!isClimbing)
            {
                groundNormal = Vector2.up;
                float fallSpeed = isWalled ? maxFallSpeed * wallFallSpeedMultiplier : maxFallSpeed;
                float jumpWallSpeed = maxFallSpeed * wallJumpSpeedMultiplier;
                if (rb2d.linearVelocityY < -fallSpeed)
                {
                    rb2d.linearVelocityY = -fallSpeed;
                }
                if (rb2d.linearVelocityY > jumpWallSpeed)
                {
                    rb2d.linearVelocityY = jumpWallSpeed;
                }
            }
            if (canStartFallTimer)
            {
                fallBackTimer = fallTimerSeconds;
                fallBackTimer /= Time.fixedDeltaTime;
                canStartFallTimer = false;
            }

            if (rb2d.linearVelocityY <= -maxFallSpeed && canStartFallTimer)
            {
                fallBackTimer = fallTimerSeconds;
                fallBackTimer /= Time.fixedDeltaTime;
                canStartFallTimer = false;
            }
        }
        


        if (wantsToRoll > 0 && !isRolling)
        {
            wantsToRoll--;
            if (isGrounded)
            {
                isRolling = true;
                StartCoroutine(DoRoll());
            }
        }
        
        rollCollider.SetActive(isRolling);
        normalCollider.SetActive(!isRolling);
        
        if(isRolling)
            return;

        if (frameCounter > 10)
        {
            frameCounter = 0;
            if (isGrounded)
                lastPosOnGround = transform.position;
            StopWithForce(1);
        }
        
        
        HandleJump();
        HandleGliding();
        HandleMovement();
        if (!canStartFallTimer)
        {
            fallBackTimer -= 1 * fallTimerMultiplierInGliding;
            if (fallBackTimer <= 0)
            {
                transform.position = lastPosOnGround;
                canStartFallTimer = true;
            }
        }
    }

   
    
    private void HandleGliding()
    {
        float glidingFallSpeed = maxFallSpeed * glidingFallSpeedMultiplier;
        if (!isWalled && isWantsToGlide && rb2d.linearVelocityY < 0)
            rb2d.linearVelocityY = -glidingFallSpeed;
    }

    private void HandleWalls()
    {
        if(Mathf.Abs(targetVelocity.x) > .05f)
            wallCheckDirection = targetVelocity.x > 0 ? Vector2.right : Vector2.left;

        float dir = wallCheckDirection.x;
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = wallLayer,
            useNormalAngle = true,
            minNormalAngle = dir > 0 ? 180 - wallAngle : -wallAngle,
            maxNormalAngle = dir > 0 ? 180 + wallAngle : wallAngle,
        };
        
        int hitCount = rb2d.Cast(Vector2.right * dir, contactFilter, hits, wallCheckRadius);
        Debug.DrawRay(transform.position, Vector2.right * dir, Color.cyan);
        isWalled = hitCount > 0;
        Vector2 newNormal = Vector2.zero;
        for (int i = 0; i < hitCount; ++i)
        {
            newNormal += hits[i].normal;
            Debug.DrawRay(hits[i].point, hits[i].normal, Color.red);
        }

        wallNormal = newNormal / hitCount;
        
        if (isWalled)
        {
            float dot = Vector2.Dot(wallNormal, inputDirection);
            if (dot > 0.2f && inputDirection.sqrMagnitude > 0.1f)
                isWalled = false;
            else
                isClimbing = true;
        }
        else
        {
            isClimbing = false;
        }

        if (isWalled && inputDirection == Vector2.zero)
            rb2d.AddForceY(-climbFallSpeed);
    }

    private void HandleGround()
    {
        if(isJumping && rb2d.linearVelocityY > 0)
        {
            isGrounded = false;
            return;
        }
        
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = groundLayer,
            useNormalAngle = true,
            maxNormalAngle = maxGroundAngle,
            minNormalAngle = minGroundAngle,
        };
        
        int hitCount = rb2d.Cast(-groundNormal , contactFilter, hits, groundCheckRadius);
        
        isGrounded = hitCount > 0;
        if (isGrounded)
        {
            Vector2 newNormal = Vector2.zero;
            for (int i = 0; i < hitCount; ++i)
                newNormal += hits[i].normal;

            groundNormal = newNormal / hitCount;
        }
    }

    private void HandleJump()
    {
        if (wantsToJump > 0)
        {
            if (isGrounded && !isJumping)
            {
                isJumping = true;
                rb2d.AddForceY(jumpForce, ForceMode2D.Impulse);
                Debug.DrawRay(transform.position, Vector2.up * jumpForce, Color.magenta, 1);
            }
            
            if (!isGrounded && isWalled) 
            {
                Vector2 direction = wallNormal.normalized * wallNormalJumpForce;
                direction += Vector2.up * (jumpForce * wallJumpForceMultiplier);
                rb2d.AddForce(direction, ForceMode2D.Impulse);
                Debug.DrawRay(transform.position, direction, Color.red, 2);
                isJumping = true;
                wallCheckDirection = wallCheckDirection == Vector2.right ? Vector2.left : Vector2.right;
            }
            isClimbing = false;
        }
    }
    
    private void HandleMovement()
    {
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1);

        if (isWalled && isClimbing)
        {
            rb2d.gravityScale = 0;
            DoClimbMovement();
        }
        else
        {
            rb2d.gravityScale = gravityScale;
            DoNormalMovement();
        }
    }
    
    private IEnumerator DoRoll()
    {
        isRolling = true;

        Vector2 dir = animatorController.FacingDirection;
        StopWithForce(initialRollSpeedModifier);
        rb2d.AddForce(dir * rollingForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(minRollTime);

        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!isGrounded)
            {
                isRolling = false;
                yield break;
            }

            if(rb2d.linearVelocityY > .2f)
                break;
            
            var dot = Vector2.Dot(groundNormal.normalized, Vector2.up);
            bool isInSlope = dot < .98f;
            if (!isInSlope)
            {
                Vector2 center = rb2d.position + groundNormal * (rollHeightCheck * .5f * 1.02f);
                Vector2 size = new Vector2(.2f, rollHeightCheck);

                Collider2D hit = Physics2D.OverlapBox(center, size, 0, wallLayer);

                Debug.DrawLine(center - size / 2, center + size / 2, Color.yellow);
                if (hit == null)
                    break;
            }
        }

        float currentTime = 0;

        while (currentTime < stopRollTime)
        {
            yield return null;
            yield return new WaitForFixedUpdate();
            
            currentTime += Time.deltaTime;
            if (!isGrounded)
            {
                isRolling = false;
                yield break;
            }
            float normalizedTime = currentTime / stopRollTime;
            StopWithForce(stopRollCurve.Evaluate(normalizedTime));
        }
        isRolling = false;
    }

    private void DoClimbMovement()
    {
        rb2d.AddForce(-wallNormal * climbForce);
        Vector2 forward = Vector2.Perpendicular(wallNormal);
        
        //float verticalAmount = Vector2.Dot(inputDirection, wallNormal);
        float horizontalAmount = Vector2.Dot(inputDirection, forward);

        float targetClimbMaxSpeed = horizontalAmount * climbMaxSpeed;
        targetVelocity = forward * targetClimbMaxSpeed;
        
        Debug.DrawRay(transform.position, forward, Color.magenta, 2);
        
        bool isGoingBackward = Vector2.Dot(targetVelocity, rb2d.linearVelocity) < 0;
        if (isGoingBackward || Mathf.Abs(horizontalAmount) < .15f)
            StopWithForce(semiTurnForceMultiplier);
        else
        {
            float lVelX = Vector2.Dot(rb2d.linearVelocity, targetVelocity.normalized);
            
            bool needAcc = Mathf.Abs(lVelX) < Mathf.Abs(targetClimbMaxSpeed);
            if (needAcc)
            {
                rb2d.AddForce(targetVelocity.normalized * climbAcceleration, ForceMode2D.Force);
            }
            else
            {
                rb2d.AddForce(-targetVelocity.normalized * climbDeceleration, ForceMode2D.Force);
            }
        }
    }

    private void DoNormalMovement()
    {
        if(inputDirection.sqrMagnitude < .1f)
        {
            if (Mathf.Abs(rb2d.linearVelocityX) >= .1f && isGrounded)
                StopWithForce(1);

            targetVelocity = Vector2.zero;
            return;
        }
        
        targetVelocity = inputDirection * maxSpeed;

        bool isGoingBackward = Vector2.Dot(targetVelocity, rb2d.linearVelocity) < 0;
        if (isGoingBackward && isGrounded)
        {
            StopWithForce(semiTurnForceMultiplier);
        }
        else
        {
            bool needAcc = Mathf.Abs(rb2d.linearVelocityX) < Mathf.Abs(targetVelocity.x);
            if (needAcc)
            {
                float accel = isGrounded ? acceleration : airAcceleration;
                rb2d.AddForceX(targetVelocity.x > 0 ? accel : -accel, ForceMode2D.Force);
            }
            else
            {
                float decel = isGrounded ? deceleration : airDeceleration;
                rb2d.AddForceX(targetVelocity.x > 0 ? -decel : decel, ForceMode2D.Force);
            }
        }
    }

    private void StopWithForce(float modifier)
    {
        if (isWalled)
        {
            Vector2 stopVelocity = -rb2d.linearVelocity * (rb2d.mass * stopForce);
            Debug.DrawRay(transform.position, stopVelocity * 20, Color.yellow);
            rb2d.AddForce(stopVelocity, ForceMode2D.Force);
        }
        else
        {
            Vector2 force = -rb2d.linearVelocity * (rb2d.mass * stopForce);
            rb2d.AddForce(force * modifier);
        }
    }
    
    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void JumpInput(InputAction.CallbackContext context)
    {
        if (context.performed)
            wantsToJump = 6;
    }
    
    public void RollingInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            wantsToRoll = 5;
            targetVelocity = wallCheckDirection;
        }
    }

    public void GlidingInput(InputAction.CallbackContext context)
    {
        isWantsToGlide = true;
        fallTimerMultiplierInGliding = fallTimerMultiplier;
        if (context.canceled)
        {
            isWantsToGlide = false;
            fallTimerMultiplierInGliding = 1;
        }
    }
}
