using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 direction;
    private int speed = 10;
    private float impulsion;
    private bool canHook;
    private bool hookInput;
    private Rigidbody2D rb2d;
    private bool isGrounded;
    private Camera playerCamera;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        impulsion = 25;
        rb2d = GetComponent<Rigidbody2D>();
        playerCamera = GetComponentInChildren<Camera>();
    }
    
    private void Update()
    {
        transform.position += new Vector3(direction.x, 0, 0) * (Time.deltaTime * speed);
        if (hookInput)
            transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);
        if (rb2d.linearVelocityY >= 0.2f || rb2d.linearVelocityY < -0.2f)
        {
            isGrounded = false;
        }
        else
        {
            isGrounded = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("CanBeHook"))
        {
            canHook = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("CanBeHook"))
        {
            canHook = false;
            ResetGravity();
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            rb2d.AddForceY(impulsion, ForceMode2D.Impulse);
        }
    }

    public void HookOn(InputAction.CallbackContext context)
    {
        if (context.performed && canHook)
        {
            hookInput = true;
            rb2d.gravityScale = 0;
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
            rb2d.gravityScale = 0.2f;
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

    private void ResetGravity()
    {
        rb2d.gravityScale = 2;
    }
}