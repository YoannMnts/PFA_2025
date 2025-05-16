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

    private void FixedUpdate()
    {
        Run();
        Fall();
    }

    private void Fall()
    {
        fallingVFX.SetActive(Movement.CurrentVelocity.y < -10);
    }

    private void Run()
    {
        runningVFX.SetActive(Mathf.Abs(Movement.CurrentVelocity.x) > 9.5f && Movement.IsGrounded);
        runningVFX.transform.localScale = new Vector3(Animator.FacingDirection.normalized.x, runningVFX.transform.localScale.y, runningVFX.transform.localScale.z);
        runningVFX.transform.rotation = Quaternion.Euler(0, Animator.FacingDirection.normalized.x * -90, 0);
    }
}
