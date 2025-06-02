using System;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSFXController : SoundObject
{
    private Player player;
    private PlayerMovement Movement => player.Movement;

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
        if (Movement.IsWalled && Mathf.Abs(Movement.CurrentVelocity.y) > 0.1f)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Jump()
    {
        if (Movement.IsJumping)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Roll()
    {
        if (Movement.IsRolling)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Glide()
    {
        if (Movement.IsGliding)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Run()
    {
        if (Movement.IsRunning)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void Walk()
    {
        if (Mathf.Abs(Movement.CurrentVelocity.x) > 0.1f && !Movement.IsRunning)
        {
            PlaySound(clips[0], SoundType.Effects, true);
        }
    }
}
