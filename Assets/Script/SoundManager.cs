using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> audioSources;
    [SerializeField] private OptionsPanel optionsPanel;
    
    public void PlaySound(AudioClip clip, SoundType type)
    {
        Debug.Log(clip);
        bool played = false;
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (audioSources[i].isPlaying == false)
            {
                audioSources[i].clip = clip;
                if (type == SoundType.Effects)
                {
                    audioSources[i].volume = optionsPanel.volumes[2]*0.1f*optionsPanel.volumes[0]*0.1f;
                }
                else if (type == SoundType.Music)
                {
                    audioSources[i].volume = optionsPanel.volumes[1]*0.1f*optionsPanel.volumes[0]*0.1f;
                }
                else if (type == SoundType.Voices)
                {
                    audioSources[i].volume = optionsPanel.volumes[3]*0.1f*optionsPanel.volumes[0]*0.1f;
                }
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
            if (type == SoundType.Effects)
            {
                audioSources[lastIndex].volume = optionsPanel.volumes[2]*optionsPanel.volumes[0];
            }
            else if (type == SoundType.Music)
            {
                audioSources[lastIndex].volume = optionsPanel.volumes[1]*optionsPanel.volumes[0];
            }
            else if (type == SoundType.Voices)
            {
                audioSources[lastIndex].volume = optionsPanel.volumes[3]*optionsPanel.volumes[0];
            }
            audioSources[lastIndex].Play();
            while (audioSources.Count >= 10)
            {
                Destroy(audioSources[0].gameObject);
                audioSources.RemoveAt(0);
            }
        }
    }
}
