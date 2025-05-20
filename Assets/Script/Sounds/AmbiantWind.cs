using System;
using UnityEngine;

public class AmbiantWind : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioSource source;
    private float distanceY;

    void Start()
    {
        distanceY = maxY-minY;
    }
    private void Update()
    {
        
        float currentY = player.transform.position.y;
        
        float v = (currentY - minY) / distanceY;
        source.volume = (float)Math.Round(v, 2);
    }
}
