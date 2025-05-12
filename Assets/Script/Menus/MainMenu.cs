using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenu : MonoBehaviour
{
    [SerializeField] StartSequence startSequence;
    [SerializeField] RectTransform[] buttons;
    [SerializeField] RectTransform outline;
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
        }
        
    }

    public void Select(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (currentLevel == 0)
            {
                DontDestroyOnLoad(startSequence.gameObject);
                StartCoroutine(startSequence.GetComponent<StartSequence>().Play());
            }
        }
        
    }

}
