using System;
using Script;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private Player player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("DeathZone");
        if (other.CompareTag("Player"))
        {
            player.Die();
        }
    }
}
