using System;
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
    public bool IsGrounded => isGrounded;
    private Vector2 CurrentVelocity => rb2d.linearVelocity;
    
    private PlayerInput playerInput;
    private Rigidbody2D rb2d;
    private Player player;



    [Header("Movement")]
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
    
    [Header("Glide check")]
    [SerializeField, Range(0f, 1f)] 
    private float glidingFallSpeedMultiplier;
    
    [Header("Glide check")]
    [SerializeField] 
    private float rollingForce;
    
    private bool isGrounded;
    private bool isJumping;
    private bool isWalled;
    private bool isWantsToGlide;
    private bool isRolling;
    private int wantsToJump;
    private int wantsToRolling;
    
    private Camera playerCamera;

    private Vector2 groundNormal;
    private Vector2 wallNormal;

    private Vector2 targetVelocity;
    private Vector2 inputDirection;
    
    private RaycastHit2D[] hits;

    private void Awake()
    {
        player = GetComponent<Player>();
        playerInput = GetComponent<PlayerInput>();
        rb2d = GetComponent<Rigidbody2D>();
        playerCamera = GetComponentInChildren<Camera>();
        hits = new RaycastHit2D[32];
    }


    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if(wantsToJump > 0)
            wantsToJump--;
        
        if(wantsToRolling > 0)
            wantsToRolling--;
        else
            isRolling = false;

        if(isGrounded)
            isJumping = false;

        HandleGround();
        HandleWalls();

        if (!isGrounded)
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
        
        HandleJump();
        HandleGliding();
        HandleRolling();
        //HandleSlopes
        HandleMovement();
    }

    private void HandleRolling()
    {
        float dir = Mathf.Sign(rb2d.linearVelocityX);
        if (wantsToRolling > 0)
        {
            if (isGrounded && !isRolling)
            {
                isRolling = true;
                rb2d.AddForceX(rollingForce * dir, ForceMode2D.Impulse);
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
        float dir = Mathf.Sign(Mathf.Abs(rb2d.linearVelocityX) <= 0.1f ? targetVelocity.x : rb2d.linearVelocityX);
        
        
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = wallLayer,
            useNormalAngle = true,
            minNormalAngle = dir > 0 ? 180 - wallAngle : -wallAngle,
            maxNormalAngle = dir > 0 ? 180 + wallAngle : wallAngle,
        };
        
        int hitCount = rb2d.Cast(Vector2.right * dir, contactFilter, hits, wallCheckRadius);
        isWalled = hitCount > 0;
        Vector2 newNormal = Vector2.zero;
        for (int i = 0; i < hitCount; ++i)
        {
            newNormal += hits[i].normal;
            Debug.DrawRay(hits[i].point, hits[i].normal, Color.red);
        }

        wallNormal = newNormal / hitCount;
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
            }
            
            if (!isGrounded && isWalled) 
            {
                Vector2 direction = wallNormal.normalized * wallNormalJumpForce;
                direction += Vector2.up * (jumpForce * wallJumpForceMultiplier);
                rb2d.AddForce(direction, ForceMode2D.Impulse);
                Debug.DrawRay(transform.position, direction, Color.red, 2);
                isJumping = true;
            }
        }
    }


    private void HandleMovement()
    {
        inputDirection = Vector2.ClampMagnitude(inputDirection, 1);
        
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
        float force = -rb2d.linearVelocity.x * rb2d.mass * stopForce;
        rb2d.AddForceX(force * modifier);
    }
    
    public void MoveInput(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
            wantsToJump = 6;
    }
/*
    public void HookOn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            hookInput = true;
        }

        if (context.canceled)
        {
         hookInput = false;  
         ResetGravity();
        }
    }
*/
    public void Rolling(InputAction.CallbackContext context)
    {
        if (context.performed)
            wantsToRolling = 60;
    }

    public void Gliding(InputAction.CallbackContext context)
    {
        isWantsToGlide = true;
        if (context.canceled)
        {
            isWantsToGlide = false;
        }
    }
}