using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] public AudioClip[] clips;
    public virtual void Start()
    {
        StartCoroutine(Research());
    }

    public void PlaySound(AudioClip clip, SoundType type)
    {
        soundManager.PlaySound(clip, type);
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