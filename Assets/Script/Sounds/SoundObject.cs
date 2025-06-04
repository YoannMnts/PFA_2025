using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public List<AudioSource> AudioSources => soundManager.AudioSources;
    
    private SoundManager soundManager;
    [SerializeField] public AudioClip[] clips;
    public virtual void Start()
    {
        StartCoroutine(Research());
    }

    public void PlaySound(AudioClip clip, SoundType type, bool unique = false)
    {
        soundManager.PlaySound(clip, type, unique);
    }

    IEnumerator Research()
    {
        soundManager = FindObjectOfType<SoundManager>();
        while (soundManager == null)
        {
            soundManager = FindObjectOfType<SoundManager>();
            yield return 1f;
        }
    }
}

public enum SoundType
{
    Music,
    Effects,
    Voices
}