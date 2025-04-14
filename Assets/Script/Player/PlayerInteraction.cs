using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class PlayerInteraction : MonoBehaviour
    {
        public bool IsInteract => isInteract;
        private bool isInteract;
        private int wantsToInteract;

        private void FixedUpdate()
        {
            IsInteracting();
        }

        private void IsInteracting()
        {
            wantsToInteract--;
            isInteract = wantsToInteract > 0;
        }


        public void InteractInput(InputAction.CallbackContext context)
        {
            wantsToInteract = 6;
        }
    }
}