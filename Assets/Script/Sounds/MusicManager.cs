using System;
using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private OptionsPanel optionsPanel;

    private void Update()
    {
        audioSource.volume = optionsPanel.volumes[0]*0.1f*optionsPanel.volumes[1]*0.1f;
    }
}
