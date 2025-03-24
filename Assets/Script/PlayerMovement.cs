using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private GameObject go;
    private Vector2 direction;
    private int speed = 10;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        go = GameObject.FindGameObjectWithTag("Player");
    }

    public void MoveInput(InputAction.CallbackContext context)
    {
        direction = context.ReadValue<Vector2>();
    }

    public void Jump()
    {
        Debug.Log("Jump");
    }

    public void HookOn()
    {
        Debug.Log("HookOn");
    }

    public void Roulade()
    {
        Debug.Log("Roulade");
    }

    private void Update()
    {
        go.transform.position += new Vector3(direction.x, direction.y, 0) * (Time.deltaTime * speed);
    }
}
