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

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
            animator = GetComponent<PlayerAnimatorController>();
        }
    }
}