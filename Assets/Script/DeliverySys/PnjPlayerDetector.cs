using System;
using UnityEngine;

public class PnjPlayerDetector : MonoBehaviour
{
    [SerializeField] private SpriteRenderer pnjSprite;
    [SerializeField] private bool right ;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            pnjSprite.flipX = right;
        }
    }
}
