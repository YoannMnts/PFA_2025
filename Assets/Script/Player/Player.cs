using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class Player : MonoBehaviour
    {
        public PlayerInput Inputs => playerInput;
        public PlayerMovement Movement => movement;
        public PlayerAnimatorController Animator => animator;
        
        
        
        [SerializeField]
        private PlayerInput playerInput;
        
        private PlayerMovement movement;
        private PlayerAnimatorController animator;

        private Vector2 lastPosOnGround;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            animator = GetComponent<PlayerAnimatorController>();
        }

        private void FixedUpdate()
        {
            if (movement.IsGrounded)
                lastPosOnGround = movement.Rb2d.position;
            
            if(movement.Rb2d.position.y < -250)
                Die();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("DeathZone"))
            {
                Die();
            }
        }

        private void Die()
        {
            movement.Rb2d.position = lastPosOnGround;
            movement.Freeze();
        }
    }
}