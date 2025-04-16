using UnityEngine;

namespace Script
{
    public abstract class PlayerInteractable : MonoBehaviour
    {
        public abstract int GetPriority();
        public abstract void Interact();
        public abstract bool CanInteract();
    }
}