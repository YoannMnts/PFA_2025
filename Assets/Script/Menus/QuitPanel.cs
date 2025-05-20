using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuitPanel : Panel
{
    public int currentLevel = 0;
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private RectTransform selectPad;
    [SerializeField] private GameObject unsavedIcon;
    [SerializeField] private bool saved = false;
    [SerializeField] private bool currentlyChoosingSave = false;
    [SerializeField] private GameObject savingPanel;
    [SerializeField] private TextMeshProUGUI lettersCountText;
    [SerializeField] private Image blackScreen;
    public int lettersCount = 0;
    

    public override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        selectPad.anchoredPosition = buttons[0].gameObject.GetComponent<RectTransform>().anchoredPosition ;
        saved = false;
        lettersCountText.text = lettersCount.ToString();
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

            StartCoroutine(MoveSelector(buttons[currentLevel].gameObject.GetComponent<RectTransform>().anchoredPosition));
        }
        PlaySound(clips[UnityEngine.Random.Range(0, clips.Length)],SoundType.Effects);
       
       
        
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
            StartCoroutine(MoveSelector(buttons[currentLevel].gameObject.GetComponent<RectTransform>().anchoredPosition));
            PlaySound(clips[UnityEngine.Random.Range(0, clips.Length)],SoundType.Effects);
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
            StartCoroutine(BackToMainMenu(2));
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
    IEnumerator MoveSelector(Vector3 destination)
    {
        float speed = 1400f;
        RectTransform rectTransform = selectPad;

        if (Vector3.Distance(destination, rectTransform.anchoredPosition) > 200)
        {
            rectTransform.anchoredPosition = destination;
        }
        else
        {
            while (Vector3.Distance(destination, rectTransform.anchoredPosition) > 5)
            {
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, destination, speed*Time.deltaTime);
                yield return null;
            }
            rectTransform.anchoredPosition = destination;
        }
    }

    IEnumerator BackToMainMenu(float time)
    {
        blackScreen.gameObject.SetActive(true);
        Color color = blackScreen.color;
        color.a = 0f;
        blackScreen.color = color;
        float speed = 1 / time;
        while (blackScreen.color.a < 1f)
        {
            color.a += Time.deltaTime * speed;
            blackScreen.color = color;
            yield return null;
        }

        SceneManager.LoadSceneAsync(1);
    }
}
