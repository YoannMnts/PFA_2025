using System;
using UnityEngine;

namespace Script
{
    public class PlayerInteraction : MonoBehaviour
    {
        private PnjInteraction pnjInteraction;
        private bool isInteract;

        private void Awake()
        {
            pnjInteraction = GetComponent<PnjInteraction>();
        }

        private void FixedUpdate()
        {
            HandleInteract();
        }

        private void HandleInteract()
        {
            if (isInteract)
                Debug.Log("Interact");
        }

        public void InteractInput(InputAction.CallbackContext context)
        {
            isInteract = true;
        }
    }
}