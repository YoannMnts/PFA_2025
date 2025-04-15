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
        public PlayerInteraction Interaction => playerInteraction;
        
        
        [SerializeField]
        private PlayerInput playerInput;
        private PlayerInteraction playerInteraction;
        private PlayerMovement movement;
        private PlayerAnimatorController animator;

        private Vector2 lastPosOnGround;
        
        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            animator = GetComponent<PlayerAnimatorController>();
            playerInteraction = GetComponent<PlayerInteraction>();
        }

        private void FixedUpdate()
        {
            if (movement.IsGrounded)
                lastPosOnGround = movement.Rb2d.position;
            
            if(movement.Rb2d.position.y < -250)
                Die();
        }
        
        public void Die()
        {
            movement.Rb2d.position = lastPosOnGround;
            movement.Freeze();
        }
    }
}