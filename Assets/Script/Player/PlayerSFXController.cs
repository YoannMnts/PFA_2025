using System;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSFXController : SoundObject
{
    private Player player;
    private PlayerMovement Movement => player.Movement;
    private int JumpIndex = 2;
    private bool canPlaySound;
    private bool playRandomVoice;

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
        RandomVoice();
    }

    private void Climb()
    {
        if (Movement.IsWalled && Mathf.Abs(Movement.CurrentVelocity.y) > 3 && !Movement.IsGrounded && !Movement.IsJumping)
        {
            PlaySound(clips[0], SoundType.Effects, true);
            playRandomVoice = true;
        }
    }

    private void Jump()
    {
        if (Movement.IsJumping && canPlaySound)
        {
            PlaySound(clips[JumpIndex], SoundType.Effects, true);
            playRandomVoice = true;
            JumpIndex += 1;
            if (JumpIndex > 4)
                JumpIndex = 2;
            canPlaySound = false;
        }
        if (!Movement.IsJumping)
            canPlaySound = true;
    }

    private void Roll()
    {
        if (Movement.IsRolling)
        {
            PlaySound(clips[5], SoundType.Effects, true);
            playRandomVoice = true;
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
            PlaySound(clips[6], SoundType.Effects, true);
        }
    }

    private void Walk()
    {
        if (Mathf.Abs(Movement.CurrentVelocity.x) > 0.1f && !Movement.IsRunning)
        {
            //PlaySound(clips[0], SoundType.Effects, true);
        }
    }

    private void RandomVoice()
    {
        if (playRandomVoice)
        {
            int randomPlay = Random.Range(1, 4);
            Debug.Log(randomPlay);
            if (randomPlay > 2)
            {
                int randomIndex = Random.Range(7, 12);
                PlaySound(clips[randomIndex], SoundType.Voices, true);
            }
            playRandomVoice = false;
        }
    }
}
