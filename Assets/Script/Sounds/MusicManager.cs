using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private OptionsPanel optionsPanel;

    void Start()
    {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
        
    }

    IEnumerator ManageMusics()
    {
        audioSource.volume = optionsPanel.volumes[1]*0.1f*optionsPanel.volumes[0]*0.1f;
        if (audioSource.isPlaying == false)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
        yield return null;
    }
    
}
