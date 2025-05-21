using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class HazelLetterBox : MonoBehaviour
{
    [SerializeField] private EndSequence endSequence;
    [SerializeField] public AboveHeadIndication aboveHeadIndication;
    bool canInteract = true;
    public bool canEnd = false;

    private void Start()
    {
        aboveHeadIndication.GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canInteract = false;
        }
    }
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (canInteract && canEnd)
            {
                canInteract = false;
                StartCoroutine(endSequence.EndingAppear());
            }
        }
    }
}
