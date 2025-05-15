using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    private SoundManager soundManager;
    [SerializeField] public AudioClip[] clips;
    public virtual void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void PlaySound(AudioClip clip, SoundType type)
    {
        soundManager.PlaySound(clip, type);
    }
}

public enum SoundType
{
    Music,
    Effects,
    Voices
}