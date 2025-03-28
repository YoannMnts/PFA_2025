using System;
using System.Collections.Generic;
using Script;
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
    private Vector2 jumpForceVector = new Vector2(20, 20);
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
    
    
    private bool isGrounded;
    private bool isJumping;
    private bool isWalled;
    private int wantsToJump;
    
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

        if(isGrounded)
            isJumping = false;

        HandleGround();

        if (!isGrounded)
        {
            groundNormal = Vector2.up;
            rb2d.linearVelocityY = Mathf.Clamp(rb2d.linearVelocityY, -maxFallSpeed, maxFallSpeed);
            wallNormal = Vector2.left;
        }
        
        HandleJump();
        //HandleSlopes
        HandleMovement();
        HandleWalls();
    }

    private void HandleWalls()
    {
        ContactFilter2D contactFilter = new ContactFilter2D()
        {
            useLayerMask = true,
            layerMask = wallLayer,
        };
        if (rb2d.linearVelocityX < 0)
        {
            int hitCount = rb2d.Cast(-wallNormal , contactFilter, hits, wallCheckRadius); //marche que si on arrive de la gauche
            isWalled = hitCount > 0;
        }
        else
        {
            int hitCount = rb2d.Cast(wallNormal , contactFilter, hits, maxSpeed);
            isWalled = hitCount > 0;
        }
        
        float dir = -Mathf.Sign(targetVelocity.x);
        if (isWalled && wantsToJump > 0) //remplacer wantsToJump
        {
            jumpForceVector.x *= dir;
            rb2d.AddForce(jumpForceVector, ForceMode2D.Impulse);
        }
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
        if (isGrounded && wantsToJump > 0 && !isJumping)
        {
            isJumping = true;   
            rb2d.AddForceY(jumpForce, ForceMode2D.Impulse);
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
        if(context.performed)
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

    public void Roulade(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
            rb2d.AddForceX(impulsion, ForceMode2D.Impulse);
    }

    public void Gliding(InputAction.CallbackContext context)
    {
        if (context.performed && rb2d.linearVelocityY < -0.2f)
        {
            rb2d.gravityScale = 0.01f;
            Debug.Log("Gliding");
            playerCamera.orthographicSize = 6;
        }

        if (context.canceled)
        {
            ResetGravity();
            Debug.Log("StopGliding");
            playerCamera.orthographicSize = 5;
        }
    }
*/
}