using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class QuitPanel : Panel
{
    public int currentLevel = 0;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private RectTransform selectPad;
    [SerializeField] private GameObject unsavedIcon;
    [SerializeField] private bool saved = false;
    [SerializeField] private bool currentlyChoosingSave = false;
    [SerializeField] private GameObject savingPanel;
    [SerializeField] private TextMeshProUGUI lettersCount;

    public override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        selectPad.anchoredPosition = buttons[0].gameObject.GetComponent<RectTransform>().anchoredPosition ;
        saved = false;
        lettersCount.text = "??";
        unsavedIcon.SetActive(true);
        currentLevel = 0;
        savingPanel.SetActive(false);
        currentlyChoosingSave = false;
    }
    public override void BottomDPad()
    {
        if (currentlyChoosingSave == false)
        {
             base.BottomDPad();
             currentLevel += 1;
            if (currentLevel >= 3) 
            { 
                currentLevel = 0;
            } 
            selectPad.anchoredPosition = buttons[currentLevel].gameObject.GetComponent<RectTransform>().anchoredPosition ;
        }
       
       
        
    }

    public override void TopDPad()
    {
        if (currentlyChoosingSave == false)
        {
            base.TopDPad();
            currentLevel -= 1;
            if (currentLevel < 0)
            {
                currentLevel = 2;
            }
            selectPad.anchoredPosition = buttons[currentLevel].gameObject.GetComponent<RectTransform>().anchoredPosition ;
        }
        
    }

    public override void SouthButton()
    {
        base.SouthButton();
        if (currentlyChoosingSave == false)
        {
            if (currentLevel == 0)
            {
                Quit();
            }
            else if (currentLevel == 1) 
            { 
                MainMenu();
            }
            else if (currentLevel == 2) 
            { 
                Save();
            }
        }
        else
        {
            saved = true;
            if (currentLevel == 0)
            {
                Quit();
            }

            if (currentLevel == 1)
            {
                MainMenu();
            }
            
        }
    }

    public override void EastButton()
    {
        base.EastButton();
        if (currentlyChoosingSave)
        {
            currentlyChoosingSave = false;
            savingPanel.SetActive(false);
            foreach (GameObject button in buttons)
            {
                button.SetActive(true);
            }
            selectPad.gameObject.SetActive(true);
        }
    }

    private void Quit()
    {
        if (saved)
        {
            ///Application.Quit();
        }
        else
        {
            currentlyChoosingSave = true;
            savingPanel.SetActive(true);
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            selectPad.gameObject.SetActive(false);
        }
    }

    private void Save()
    {
        saved = true;
        unsavedIcon.SetActive(false);
    }

    private void MainMenu()
    {
        if (saved)
        {
            /// load scene
        }
        else
        {
            currentlyChoosingSave = true;
            savingPanel.SetActive(true);
            foreach (GameObject button in buttons)
            {
                button.SetActive(false);
            }
            selectPad.gameObject.SetActive(false);
        }
    }
}
