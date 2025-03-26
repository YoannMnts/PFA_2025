using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Vector2 direction;
    private int speed = 10;
    private float jumpForce;
    private bool canHook;
    private bool hookInput;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpForce = 20;
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
            GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GetComponent<Rigidbody2D>().AddForceY(jumpForce, ForceMode2D.Impulse);
        }
    }

    public void HookOn(InputAction.CallbackContext context)
    {
        if (context.performed && canHook)
        {
            hookInput = true;
            GetComponent<Rigidbody2D>().gravityScale = 0.01f;
        }

        if (context.canceled)
        {
         hookInput = false;  
         GetComponent<Rigidbody2D>().gravityScale = 2;
        }
    }

    public void Roulade()
    {
        Debug.Log("Roulade");
    }

    private void Update()
    {
        transform.position += new Vector3(direction.x, 0, 0) * (Time.deltaTime * speed);
        if (hookInput)
            transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);
    }
}