using System;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Vector2 lastPosition;
    private Vector2 currentPosition;
    
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastPosition = currentPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (animator != null)
        {
            rigidbody2D.linearVelocity = animator.velocity;
        }
        else
        {

            lastPosition = currentPosition;
            currentPosition = transform.position;

            Vector2 delta = (currentPosition - lastPosition) * Time.fixedDeltaTime;
            rigidbody2D.linearVelocity = delta;
            Debug.Log((currentPosition - lastPosition));
        }
    }
}
