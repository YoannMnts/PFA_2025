using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Script
{
    public class PlayerInteraction : MonoBehaviour
    {
        public bool HasInteractions => playerInteractables.Count > 0;
        private List<PlayerInteractable> playerInteractables;
        
        private void Awake()
        {
            playerInteractables = new List<PlayerInteractable>();
        }


        public void InteractInput(InputAction.CallbackContext context)
        {
            Debug.Log("InteractInput");
            PlayerInteractable currentPlayerInteractable = null;
            foreach (PlayerInteractable interactable in playerInteractables)
            {
                if (currentPlayerInteractable == null ||
                    currentPlayerInteractable.GetPriority() < interactable.GetPriority())
                {
                    if (interactable.CanInteract())
                        currentPlayerInteractable = interactable;
                }
            }

            if (currentPlayerInteractable != null)
            {
                currentPlayerInteractable.Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerInteractable interactable))
            {
                playerInteractables.Add(interactable);
                Debug.Log($"Enter {interactable.name}");
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out PlayerInteractable interactable))
            {
                playerInteractables.Remove(interactable);
                Debug.Log($"Exit {interactable.name}");
            }
        }
    }
}