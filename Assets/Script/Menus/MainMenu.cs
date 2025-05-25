using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] StartSequence startSequence;
    [SerializeField] RectTransform[] buttons;
    [SerializeField] RectTransform outline;
    [SerializeField] Image blackBg;
    [SerializeField] SoundManagerNoVolume soundManager;
    [SerializeField] private AudioClip[] clips;
    private int currentLevel;

    void Start()
    {
        currentLevel = 0;
        outline.anchoredPosition = buttons[currentLevel].anchoredPosition;
    }

    public void DPadDown(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (currentLevel < buttons.Length -1)
            {
                
                currentLevel++; 
                outline.anchoredPosition = buttons[currentLevel].anchoredPosition;
            }
            else
            { 
                currentLevel = 0; 
                outline.anchoredPosition = buttons[currentLevel].anchoredPosition;
            }
            soundManager.PlaySound(clips[0]);
        }
        
    }

    public void DPadUp(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
           if (currentLevel > 0)
           {
               currentLevel--;
               outline.anchoredPosition = buttons[currentLevel].anchoredPosition;
           }
           else
           {
               currentLevel = buttons.Length - 1;
               outline.anchoredPosition = buttons[currentLevel].anchoredPosition;
           } 
           soundManager.PlaySound(clips[0]);
        }
        
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            soundManager.PlaySound(clips[1]);
            if (currentLevel == 0)
            {
                DontDestroyOnLoad(startSequence.gameObject);
                StartCoroutine(startSequence.GetComponent<StartSequence>().Play());
            }
            else if (currentLevel == 1)
            {
                StartCoroutine(Appear());
            }
            else if (currentLevel == 2)
            {
                Application.Quit();
            }
        }
        
    }

    IEnumerator Appear()
    {
        blackBg.gameObject.SetActive(true);
        Color color = blackBg.color;
        color.a = 0;
        blackBg.color = color;
        while (color.a < 1)
        {
            color.a += Time.deltaTime / 2;
            blackBg.color = color;
            yield return null;
        }
        SceneManager.LoadScene(2);
    }

}
