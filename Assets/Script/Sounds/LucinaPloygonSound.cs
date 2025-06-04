using System;
using Unity.VisualScripting;
using UnityEngine;

public class LucinaPloygonSound: PolygonSound
{
    [SerializeField] AudioSource musicSource;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            musicSource.volume = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            musicSource.volume = optionsPanel.volumes[0]*0.1f*optionsPanel.volumes[1]*0.1f;
        }
        
    }
}
