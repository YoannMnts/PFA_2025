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
    public bool IsWalled => isWalled;
    public bool IsRolling => isRolling;
    public bool IsJumping => isJumping;
    public bool IsGliding => isGliding;
    public Rigidbody2D Rb2d => rb2d;
    #endregion
    
    private PlayerAnimatorController animatorController;
    private PlayerInput playerInput;
    private Rigidbody2D rb2d;
    private Player player;



    [Header("Movement")]
    [SerializeField,Tooltip("The normal collider box")]
    private GameObject normalCollider;
    [SerializeField, Tooltip("The collider for the player when running")]
    private GameObject runningCollider;
    [SerializeField, Tooltip("Current gravity scale")] 
    private float gravityScale = 8;
    [SerializeField, Tooltip("The acceleration applied on the player")]
    private float acceleration;
    [SerializeField, Tooltip("The acceleration applied on the player in the air")]
    private float airAcceleration;
    [SerializeField, Tooltip("The acceleration deceleration on the player")]
    private float deceleration;
    [SerializeField, Tooltip("The deceleration applied on the player in the air")]
    private float airDeceleration;

    [SerializeField, Tooltip("The force applied on the player for stop him")]
    private float stopForce;
    [SerializeField, Range(1, 4), Tooltip("The force multiplier of the stopForce for do a smooth turn")] 
    private float semiTurnForceMultiplier;
    
    [SerializeField, Tooltip("The max speed of the player")]
    private float maxSpeed = 10;
    [SerializeField, Tooltip("The force applied on the players for do a jump")]
    private float jumpForce;
    [SerializeField, Tooltip("The max speed of the player when he fall off")] 
    private float maxFallSpeed = 10;
    
    [SerializeField, Tooltip("The min magnitude for going backward in slopes")] 
    private float goingBackwardInSlopes;
    
    [Header("Ground check")]
    [SerializeField, Tooltip("The layer of the ground")]
    private LayerMask groundLayer;
    [SerializeField, Range(0f, 1f), Tooltip("The radius for check if a ground is under the player")]
    private float groundCheckRadius;
    [SerializeField, Tooltip("filter the results to only include contacts with collision normal angles that are greater than this angle")]
    private float minGroundAngle;
    [SerializeField, Tooltip("filter the results to only include contacts with collision normal angles that are less than this angle")]
    private float maxGroundAngle;
    
    [Header("Wall check")]
    [SerializeField, Tooltip("The layer of the wall")]
    private LayerMask wallLayer;
    [SerializeField, Range(0f, 1f), Tooltip("The height of the box who check if a wall is at the player's feet")]
    private float wallCheckBoxHeight;
    [SerializeField, Range(0f, 1f), Tooltip("How the box comes out of the player")]
    private float wallCheckDistance;
    [SerializeField, Tooltip("The angle for stay on wall/ground")]
    private float wallAngle;
    [SerializeField, Tooltip("The force applied on the players for wall jump")]
    private float wallNormalJumpForce;
    [SerializeField, Range(0f, 1f), Tooltip("The multiplier of the force for wall jump")]
    private float wallJumpForceMultiplier;
    [SerializeField, Range(0f, 1f), Tooltip("The force applied on the player for make the max fall speed")]
    private float wallFallSpeedMultiplier;
    [SerializeField, Range(0f, 2f), Tooltip("The multiplier of the jump speed for make the max speed in wall jump")]
    private float wallJumpSpeedMultiplier;
    
    [Header("Climb")]
    [SerializeField, Tooltip("The force for climbing")]
    private float climbForce;
    [SerializeField, Tooltip("The max speed of the climbing")]
    private float climbMaxSpeed;
    [SerializeField, Tooltip("The acceleration of the climbing")]
    private float climbAcceleration;
    [SerializeField, Tooltip("The deceleration of the climbing")]
    private float climbDeceleration;
    [SerializeField, Range(1f, 100f), Tooltip("The force applied on the player for fall when he's walled")] 
    private float climbFallSpeed;
    
    [Header("Glide")]
    [SerializeField, Range(0f, 1f), Tooltip("The fall speed when the player is gliding")] 
    private float glidingFallSpeedMultiplier;
    [SerializeField, Range(1f, 2f), Tooltip("The multiplier to reach maximum speed when the player is gliding")] 
    private float glidingSpeedMultiplier;

    [Header("Roll")] 
    [SerializeField, Tooltip("The collider for the player when rolling")]
    private GameObject rollCollider;
    [SerializeField, Tooltip("The force for stop the player when rolling")]
    private float initialRollSpeedModifier;
    [SerializeField, Tooltip("The force for do the roll")] 
    private float rollingForce;
    [SerializeField, Tooltip("The time of the roll")] 
    private float minRollTime;
    [SerializeField, Tooltip("The height of box for check if there is a ceiling")] 
    private float rollHeightCheck;
    [SerializeField, Tooltip("The time to stop the player for do smooth stop")] 
    private float stopRollTime;
    [SerializeField, Tooltip("The curve for make the smooth stop roll")] 
    private AnimationCurve stopRollCurve;
    
    private bool isGrounded;
    private bool isJumping;
    private bool isWalled;
    private bool isWantsToGlide;
    private bool isRolling;
    private bool isEndRolling;
    private bool isGliding;
    
    private int wantsToJump;
    private int wantsToRoll;
    
    private Camera playerCamera;

    private Vector2 groundNormal;
    private Vector2 wallNormal;

    private Vector2 targetVelocity;
    private Vector2 inputDirection;
    private Vector2 wallCheckDirection;
    private RaycastHit2D[] hits;

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
    }

    private void FixedUpdate()
    {
        if(wantsToJump > 0)
            wantsToJump--;
        
        if(wantsToRoll > 0)
            wantsToRoll--;
        if (isGrounded)
        {
            isJumping = false;
        }
        
        HandleGround();
        HandleWalls();

        if (rb2d.linearVelocityY < 0)
            isJumping = false;
        
        if (!isGrounded)
        {
            if (!isWalled)
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
        }
        


        if (wantsToRoll > 0 && !isRolling)
        {
            wantsToRoll--;
            if (!isWalled)
            {
                isRolling = true;
                StartCoroutine(DoRoll());
            }
        }
        
        rollCollider.SetActive(isRolling);
        normalCollider.SetActive((!isRolling || isEndRolling) && Mathf.Abs(CurrentVelocity.x) < 8);
        runningCollider.SetActive(Mathf.Abs(CurrentVelocity.x) > 8);
        if(isRolling)
            return;

        HandleJump();
        HandleGliding();
        HandleMovement();
    }

   
    
    private void HandleGliding()
    {
        float glidingFallSpeed = maxFallSpeed * glidingFallSpeedMultiplier;
        if (!isWalled && isWantsToGlide && rb2d.linearVelocityY < 0 && !isGrounded)
        {
            rb2d.linearVelocityY = -glidingFallSpeed;
            maxSpeed = glidingSpeedMultiplier * 10;
            isGliding = true;
        }
        else
        {
            maxSpeed = 10;
            isGliding = false;
        }
    }

    private void HandleWalls()
    {
        if (isWalled)
            wallCheckDirection = -wallNormal;
        else if(isJumping)
            wallCheckDirection = CurrentVelocity.x > 0 ? Vector2.right : Vector2.left;
        else if (Mathf.Abs(targetVelocity.x) > .05f)
            wallCheckDirection = targetVelocity.x > 0 ? Vector2.right : Vector2.left;
        else
        {
            isWalled = false;
            wallNormal = Vector2.zero;
            return;
        }

        float dir = wallCheckDirection.x;
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = wallLayer,
            useNormalAngle = true,
            minNormalAngle = dir > 0 ? 180 - wallAngle : -wallAngle,
            maxNormalAngle = dir > 0 ? 180 + wallAngle : wallAngle,
        };
        
        //int hitCount = rb2d.Cast(Vector2.right * dir, contactFilter, hits, wallCheckRadius);
        int hitCount = Physics2D.BoxCast(
            (Vector2)transform.position + Vector2.up * (wallCheckBoxHeight * 0.58f),
            Vector2.one * wallCheckBoxHeight,
            0,
            Vector2.right * dir,
            contactFilter, 
            hits,
            wallCheckDistance);
        isWalled = hitCount > 0;
        Vector2 newNormal = Vector2.zero;
        for (int i = 0; i < hitCount; ++i)
        {
            newNormal += hits[i].normal;
            Debug.DrawRay(hits[i].point, hits[i].normal, Color.red);
        }

        if(hitCount == 0)
            wallNormal = Vector2.zero;
        else
            wallNormal = newNormal / hitCount;
        
        if (isWalled && !isGrounded)
        {
            isJumping = false;
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
        if (isWalled)
            isJumping = false;
        if (wantsToJump > 0)
        {
            if (isGrounded && !isJumping)
            {
                isJumping = true;
                rb2d.linearVelocityY = 0;
                rb2d.AddForceY(jumpForce, ForceMode2D.Impulse);
                //Debug.DrawRay(transform.position, Vector2.up * jumpForce, Color.magenta, 1);
            }
            
            if (!isGrounded && isWalled) 
            {
                Vector2 direction = wallNormal.normalized * wallNormalJumpForce;
                direction += Vector2.up * (jumpForce * wallJumpForceMultiplier);
                rb2d.linearVelocityY = 0;
                rb2d.AddForce(direction, ForceMode2D.Impulse);
                //Debug.DrawRay(transform.position, direction, Color.red, 2);
                isJumping = true;
                wallCheckDirection = wallCheckDirection == Vector2.right ? Vector2.left : Vector2.right;
            }
            wantsToJump = 0;
        }
    }
    
    private void HandleMovement()
    {
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1);

        if (isWalled && !isGrounded)
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
            //if (!isGrounded)
            //{
                //isRolling = false;
                //yield break;
            //}

            if(rb2d.linearVelocityY > .2f)
                break;
            
            var dot = Vector2.Dot(groundNormal.normalized, Vector2.up);
            bool isInSlope = dot < .98f;
            if (!isInSlope)
            {
                Vector2 center = rb2d.position + groundNormal * (rollHeightCheck * .5f * 1.02f);
                Vector2 size = new Vector2(.2f, rollHeightCheck);

                Collider2D hit = Physics2D.OverlapBox(center, size, 0, wallLayer);

                //Debug.DrawLine(center - size / 2, center + size / 2, Color.yellow);
                if (hit == null)
                    break;
            }
        }

        float currentTime = 0;

        while (currentTime < stopRollTime)
        {
            isEndRolling = true;
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
        isEndRolling = false;
    }

    private void DoClimbMovement()
    {
        rb2d.AddForce(-wallNormal * climbForce);
        Vector2 forward = Vector2.Perpendicular(wallNormal);
        
        //float verticalAmount = Vector2.Dot(inputDirection, wallNormal);
        float horizontalAmount = Vector2.Dot(inputDirection, forward);
        float targetClimbMaxSpeed = horizontalAmount * climbMaxSpeed;
        targetVelocity = forward * targetClimbMaxSpeed;
        
        //Debug.DrawRay(transform.position, forward, Color.magenta, 2);
        
        var dot = Vector2.Dot(targetVelocity.normalized, rb2d.linearVelocity.normalized);
        bool isGoingBackward = dot < 0 && rb2d.linearVelocity.sqrMagnitude > goingBackwardInSlopes;
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
        inputDirection.y = 0;
        if(inputDirection.sqrMagnitude < .1f)
        {
            if (Mathf.Abs(rb2d.linearVelocityX) >= .1f && isGrounded)
                StopWithForce(1);

            targetVelocity = Vector2.zero;
            return;
        }
        
        targetVelocity = Vector2.Perpendicular(groundNormal) * (-inputDirection.x * maxSpeed);

        var dot = Vector2.Dot(targetVelocity.normalized, rb2d.linearVelocity.normalized);
        bool isGoingBackward = dot < 0 && rb2d.linearVelocity.sqrMagnitude > goingBackwardInSlopes;
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
            else if (Mathf.Abs(rb2d.linearVelocityX) < maxSpeed)
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
            //Debug.DrawRay(transform.position, stopVelocity * 20, Color.yellow);
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
        if (context.performed && !player.Interaction.HasInteractions)
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
        if (context.canceled)
        {
            isWantsToGlide = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + Vector2.up * (wallCheckBoxHeight * 0.58f) + Vector2.right * wallCheckDistance, Vector2.one * wallCheckBoxHeight);
    }

    public void Freeze()
    {
        StopAllCoroutines();
        isJumping = false;
        isGrounded = false;
        isEndRolling = false;
        isWantsToGlide = false;
        targetVelocity = Vector2.zero;
        rb2d.linearVelocity = Vector2.zero;
        rb2d.angularVelocity = 0;
    }
}
