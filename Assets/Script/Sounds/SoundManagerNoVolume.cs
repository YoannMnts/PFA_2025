using System.Collections.Generic;
using UnityEngine;

public class SoundManagerNoVolume : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    
    public void PlaySound(AudioClip clip)
    {
        bool played = false;
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying == false)
            {
                audioSources[i].clip = clip;
                audioSources[i].Play();
                played = true;
                break;
            }
        }
        if (played == false)
        {
            GameObject obj = new GameObject();
            obj.AddComponent<AudioSource>();
            audioSources.Add(obj.GetComponent<AudioSource>());
            int lastIndex = audioSources.Count - 1;
            audioSources[lastIndex].clip = clip;
            audioSources[lastIndex].Play();
            while (audioSources.Count >= 10)
            {
                Destroy(audioSources[0].gameObject);
                audioSources.RemoveAt(0);
            }
        }
    }
}
