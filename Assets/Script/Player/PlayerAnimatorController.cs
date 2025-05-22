using System;
using UnityEngine;

namespace Script
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        private static readonly int JumpTrigger = Animator.StringToHash("Jump");
        private static readonly int IsClimbing = Animator.StringToHash("IsClimbing");
        private static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        private static readonly int IsGliding = Animator.StringToHash("IsGliding");
        private static readonly int IsRolling = Animator.StringToHash("IsRolling");
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int StayOnWall = Animator.StringToHash("StayOnWall");
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
        
        private bool wasJumping;

        private void Awake()
        {
            player = GetComponent<Player>();
            facingDirection = Vector2.right;
        }


        private void FixedUpdate()
        {
            HandleFacing();
            
            if(!wasJumping && Movement.IsJumping)
                animator.SetTrigger(JumpTrigger);
                
            
            wasJumping = Movement.IsJumping;
            
            animator.SetBool(IsGrounded, Movement.IsGrounded);
            animator.SetBool(IsClimbing, Mathf.Abs(Movement.CurrentVelocity.y) > .15f && Movement.IsWalled && !Movement.IsGrounded);
            animator.SetBool(IsRunning, Movement.IsRunning);
            animator.SetBool(IsWalking, Mathf.Abs(Movement.CurrentVelocity.x) > 2f);
            animator.SetFloat(VerticalVelocity, Movement.CurrentVelocity.y);
            animator.SetBool(IsGliding, Movement.IsGliding);
            animator.SetBool(IsRolling, Movement.IsRolling);
            animator.SetBool(StayOnWall,Movement.IsWalled && !Movement.IsGrounded);
        }
        

        private void HandleFacing()
        {
            Vector2 up = Vector2.up;
            if (Movement.IsWalled && !Movement.IsGrounded)
                up = Movement.WallNormal;
            if (Movement.IsGrounded || Movement.IsWallJumping)
            {
                up = Movement.GroundNormal;
            }
            if (!Movement.IsWalled || Movement.IsGrounded)
                spriteRenderer.transform.up = Vector2.Lerp(spriteRenderer.transform.up, up,rotationSmoothness * Time.deltaTime);
            
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
            if (Movement.IsWallJumping)
                spriteRenderer.flipX = Movement.WallCheckDirection.x < 0;
            if (Movement.IsGliding)
                spriteRenderer.flipX = Movement.CurrentVelocity.x < 0;
        }
    }
}