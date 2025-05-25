using UnityEngine;
using UnityEngine.UI;

public class Tutorial : SoundObject
{
    public GameObject tutoImage;

    void Start()
    {
        tutoImage.SetActive(false);
    }

    public void Open()
    {
        Debug.Log("Tutorial Open");
        if (tutoImage.activeInHierarchy)
        {
            tutoImage.SetActive(false);
            PlaySound(clips[0],SoundType.Effects);
        }
        else
        {
            PlaySound(clips[0],SoundType.Effects);
            tutoImage.SetActive(true);
        }
    }

    public void Close()
    {
        if (tutoImage.activeInHierarchy)
        {
            PlaySound(clips[0],SoundType.Effects);
            tutoImage.SetActive(false);
        }
    }
}
