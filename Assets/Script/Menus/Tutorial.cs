using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Tutorial : SoundObject
{
    public GameObject tutoImage;

    void Start()
    {
        base.Start();
        tutoImage.SetActive(false);
    }

    public void Open(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
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
        
    }

    public void Close()
    {
        if (tutoImage.activeInHierarchy)
        {
            tutoImage.SetActive(false);
        }
    }
}
