using System;
using UnityEngine;

public class PlayerAnimationCallbacks : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public void OnJumpEvent(AnimationEvent @event)
    {
        playerMovement.ApplyJumpForce();
    }
}
