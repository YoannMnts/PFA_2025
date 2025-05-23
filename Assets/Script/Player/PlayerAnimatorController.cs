﻿using System;
using UnityEngine;

namespace Script
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private PlayerMovement Movement => player.Movement;
        
        [Header("Refs")] 
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField] 
        private Animator animator;
        
        [SerializeField]
        private float rotationSmoothness;

        private Player player;

        private Vector2 facingDirection;
        
        public Vector2 FacingDirection => facingDirection;

        private void Awake()
        {
            player = GetComponent<Player>();
            facingDirection = Vector2.right;
        }

        private void FixedUpdate()
        {
            HandleFacing();
        }

        private void HandleFacing()
        {
            Vector2 up = Vector2.up;
            if (Movement.IsClimbing && Movement.IsWalled && !Movement.IsGrounded)
                up = Movement.WallNormal;
            else if (Movement.IsGrounded)
                up = Movement.GroundNormal;
            
            spriteRenderer.transform.up = Vector2.Lerp(spriteRenderer.transform.up, up,rotationSmoothness * Time.deltaTime);

            Vector2 perp = Vector2.Perpendicular(up);
            float dot = Vector2.Dot(Movement.InputDirection, perp);
            
            if(Mathf.Abs(dot) > 0.1f && (Movement.IsGrounded || Movement.IsClimbing))
            {
                facingDirection = dot < 0 ? -perp : perp;
                spriteRenderer.flipX = dot < 0;
            }
            else
                facingDirection = spriteRenderer.flipX ? -perp : perp;
        }
    }
}