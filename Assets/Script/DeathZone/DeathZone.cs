using System;
using Script;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("DeathZone");
            player.Die();
        }
    }
}
