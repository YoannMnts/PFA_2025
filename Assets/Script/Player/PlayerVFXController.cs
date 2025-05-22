using System;
using Script;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerVFXController : MonoBehaviour
{
    [SerializeField]
    private Player player;
    private PlayerMovement Movement => player.Movement;
    private PlayerAnimatorController Animator => player.Animator;
    [SerializeField]
    private GameObject runningVFX;
    [SerializeField]
    private GameObject fallingVFX;
    
    private ParticleSystem runningParticles;
    private ParticleSystem fallingParticles;

    private void Start()
    {
        runningParticles = runningVFX.GetComponent<ParticleSystem>();
        fallingParticles = fallingVFX.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        Run();
        Fall();
    }

    private void Fall()
    {
        if (Movement.CurrentVelocity.y < -9 && !Movement.IsGrounded && !fallingParticles.isPlaying)
            fallingParticles.Play();
        else if (Movement.IsGrounded || Movement.IsGliding || Movement.IsWalled || Movement.IsRolling)
            fallingParticles.Stop();
    }

    private void Run()
    {
        if (Movement.IsRunning && Movement.IsGrounded && !runningParticles.isPlaying)
            runningParticles.Play();
        else if (!Movement.IsRunning || !Movement.IsGrounded)
            runningParticles.Stop();
        if (runningParticles.isPlaying)
        {
            runningVFX.transform.localScale = new Vector3(Animator.FacingDirection.normalized.x, runningVFX.transform.localScale.y, runningVFX.transform.localScale.z);
            runningVFX.transform.rotation = Quaternion.Euler(0, Animator.FacingDirection.normalized.x * -90, 0);
        }
    }
}
