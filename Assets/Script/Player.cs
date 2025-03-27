using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class Player : MonoBehaviour
    {
        public PlayerInput Inputs => playerInput;
        public PlayerMovement Movement => movement;
        
        
        
        [SerializeField]
        private PlayerInput playerInput;
        
        private PlayerMovement movement;

        private void Awake()
        {
            movement = GetComponent<PlayerMovement>();
        }
    }
}