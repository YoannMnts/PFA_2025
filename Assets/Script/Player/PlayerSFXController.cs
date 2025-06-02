using System;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSFXController : SoundObject
{
    private Player player;
    private PlayerMovement Movement => player.Movement;
    private int JumpIndex = 5;

    private void Awake()
    {
        player = gameObject.GetComponent<Player>();
    }

    private void Update()
    {
        Walk();
        Run();
        Glide();
        Roll();
        Jump();
        Climb();
    }

    private void Climb()
    {
        if (Movement.IsWalled && Mathf.Abs(Movement.CurrentVelocity.y) > 0.1f && !Movement.IsGrounded)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Jump()
    {
        if (Movement.IsJumping)
        {
            PlaySound(clips[JumpSFXChoice()], SoundType.Effects, true);
        }
    }

    private int JumpSFXChoice()
    {
        if (JumpIndex >= 9)
            JumpIndex = 3;
        JumpIndex += 2;
        return Mathf.Clamp(JumpIndex, 5, 9);
    }

    private void Roll()
    {
        if (Movement.IsRolling)
        {
            PlaySound(clips[12], SoundType.Effects, true);
        }
    }

    private void Glide()
    {
        if (Movement.IsGliding)
        {
            //PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Run()
    {
        if (Movement.IsRunning && !Movement.IsRolling && Movement.IsGrounded)
        {
            Debug.Log("ici");
            PlaySound(clips[14], SoundType.Effects, true);
        }
    }

    private void Walk()
    {
        if (Mathf.Abs(Movement.CurrentVelocity.x) > 0.1f && !Movement.IsRunning)
        {
            //PlaySound(clips[0], SoundType.Effects, true);
        }
    }
}
