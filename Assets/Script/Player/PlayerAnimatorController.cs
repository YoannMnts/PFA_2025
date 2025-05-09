using System;
using UnityEngine;

namespace Script
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsClimbing = Animator.StringToHash("IsClimbing");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        private static readonly int IsGliding = Animator.StringToHash("IsGliding");
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
            
            animator.SetBool(IsGrounded, Movement.IsGrounded);
            animator.SetBool(IsJumping, Movement.IsJumping);
            animator.SetBool(IsClimbing, Movement.IsWalled && !Movement.IsGrounded);
            animator.SetBool(IsRunning, Mathf.Abs(Movement.CurrentVelocity.x) > 0);
            animator.SetFloat(VerticalVelocity, Movement.CurrentVelocity.y);
            animator.SetBool(IsGliding, Movement.IsGliding);
            animator.SetBool("IsRolling", Movement.IsRolling);
            Debug.Log(Movement.CurrentVelocity.y);
        }
        

        private void HandleFacing()
        {
            Vector2 up = Vector2.up;
            if (Movement.IsWalled && !Movement.IsGrounded)
                up = Movement.WallNormal;
            else if (Movement.IsGrounded)
                up = Movement.GroundNormal;
            
            //spriteRenderer.transform.up = Vector2.Lerp(spriteRenderer.transform.up, up,rotationSmoothness * Time.deltaTime);

            Vector2 perp = Vector2.Perpendicular(up);
            float dot = Vector2.Dot(Movement.InputDirection, perp);
            
            if(Mathf.Abs(dot) > 0.1f)
            {
                if (!Movement.IsRolling && !Movement.IsGliding)
                {
                    facingDirection = dot < 0 ? -perp : perp;
                    spriteRenderer.flipX = dot > 0;
                }
            }
            if (Movement.IsWalled)
                spriteRenderer.flipX = Movement.WallNormal.x > 0;
        }
    }
}