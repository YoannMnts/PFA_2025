using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class PlayerInteraction : MonoBehaviour
    {
        public bool IsInteract => isInteract;
        private bool isInteract;
        
        public void InteractInput(InputAction.CallbackContext context)
        {
            isInteract = true;
            if(context.canceled)
                isInteract = false;
        }
    }
}