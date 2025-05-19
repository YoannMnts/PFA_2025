using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class LettersPanel : Panel
{
    [SerializeField] DeliveryManager deliveryManager;
    [SerializeField] GameObject letterTemplate;
    [SerializeField] DirectionHelp directionHelp;
    [SerializeField] private GameObject moreUnderIndication;
    [SerializeField] private GameObject moreOverIndication;
    [SerializeField] private RectTransform readingSheet;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private TextMeshProUGUI letterAuthor;
    public GameObject[] letters;
    private GameObject[] currentLetters;
    private LetterUI[] currentLettersData;
    [SerializeField] private RectTransform selectionPad;
    [SerializeField] Vector3 lettersBasePosition;
    [SerializeField] Vector3 lettersOffset;
    [SerializeField] int lettersByPage ;
    private int lettersCount;
    private int currentLevel;
    private int currentPage;
    public int[] pinnedCoordinates;
    private bool isReading;
    private int maxIntToDeliver;

    public override void Close()
    {
        readingSheet.anchoredPosition = new Vector3(-743f,-1000f,0);
        isReading = false;
        foreach (GameObject letter in letters)
        {
            Destroy(letter);
        }
        base.Close();
    }

    public override void Open()
    {
        base.Open();
        lettersCount = deliveryManager.ActiveLetter.Count + deliveryManager.completedLetters.Count;
        maxIntToDeliver = deliveryManager.ActiveLetter.Count ;
        readingSheet.anchoredPosition = new Vector3(-743f,-1000f,0);
        isReading = false;
        currentLevel = 0;
        currentPage = 0;
        letters = new GameObject[lettersCount];
        currentLetters = new GameObject[lettersByPage];
        for (int i = 0; i < lettersCount; i++)
        {
            GameObject letter = Instantiate(letterTemplate, panel.transform);
            letters[i] = letter;
            if (i < maxIntToDeliver)
            {
                letter.GetComponent<LetterUI>().SetUp(deliveryManager.ActiveLetter[i].letterData, false);
            }
            else
            {
                letter.GetComponent<LetterUI>().SetUp(deliveryManager.completedLetters[i-maxIntToDeliver], true);
            }
            Debug.Log(i);
        }
        
        DisplayLetters();
        if (lettersCount > 0)
        {
            selectionPad.position = letters[0].GetComponent<RectTransform>().position;
        }
        else
        {
            selectionPad.position = new Vector3(1000,1000,1000);
        }
    }

    public override void Awake()
    {
        base.Awake();
        currentLevel = 0;
        currentPage = 0;
        lettersCount = deliveryManager.ActiveLetter.Count;
        pinnedCoordinates = null;
        letters = new GameObject[lettersCount];
        currentLetters = new GameObject[lettersByPage];
    }
    public override void TopDPad()
    {
        base.TopDPad();
        if (isReading == false)
        {
            currentLevel--;
            if (currentLevel < 0) 
            { 
                if (currentPage == 0) 
                { 
                    currentLevel = 0;
                }
                else 
                { 
                    currentLevel = lettersByPage - 1; 
                    currentPage--; 
                    DisplayLetters();
                }
            }

            if (lettersCount > 0)
            {
                StartCoroutine(MoveSelector(letters[currentLevel + (currentPage * lettersByPage)]
                    .GetComponent<RectTransform>().anchoredPosition));
            }
            else
            {
                selectionPad.position = new Vector3(1000,1000,1000);
            }
        }
        
        
    }

    public override void BottomDPad()
    {
        base.BottomDPad();
        if (isReading == false)
        {
             currentLevel++;
             if ((currentPage*lettersByPage)+currentLevel >= lettersCount) 
             { 
                 currentLevel = currentLevel-1;
             }
             else if (currentLevel >= lettersByPage) 
             { 
                 currentLevel = 0; 
                 currentPage++; 
                 DisplayLetters();
             } 
            
             if (lettersCount > 0)
             {
                 selectionPad.position = letters[currentLevel+ (currentPage*lettersByPage)].GetComponent<RectTransform>().position;
             }
             else
             {
                 selectionPad.position = new Vector3(1000,1000,1000);
             }
        }
        PlaySound(clips[UnityEngine.Random.Range(3, clips.Length)],SoundType.Effects);
    }

    public override void SouthButton()
    {
        base.SouthButton();
        Pin(currentLevel);
    }

    public override void WestButton()
    {
        base.WestButton();  
        if (lettersCount > 0)
        {
            Read();
        }
        
    }

    public void DisplayLetters()
    {
        if ((currentPage+1)*lettersByPage < lettersCount)
        {
            moreUnderIndication.SetActive(true);
        }
        else
        {
            moreUnderIndication.SetActive(false);
        }

        if (currentPage > 0)
        {
            moreOverIndication.SetActive(true);
        }
        else
        {
            moreOverIndication.SetActive(false);
        }
        foreach (GameObject letter in currentLetters)
        {
            if (letter != null)
            {
                letter.SetActive(false);
            }
        }
        currentLetters = new GameObject[lettersByPage];
        currentLettersData = new LetterUI[lettersByPage];
        for (int j = 0; j < lettersByPage; j++)
        {
            if ((currentPage * lettersByPage) + j < lettersCount)
            {
                letters[(currentPage*lettersByPage)+j].SetActive(true);
                currentLetters[j] = letters[(currentPage*lettersByPage)+j];
                currentLettersData[j] = currentLetters[j].gameObject.GetComponent<LetterUI>();
                if (pinnedCoordinates != null)
                {
                    if (pinnedCoordinates[0] == j && pinnedCoordinates[1] == currentPage)
                    {
                        currentLettersData[j].GetPinned();
                    }
                }
                letters[(currentPage*lettersByPage)+j].GetComponent<RectTransform>().anchoredPosition = lettersBasePosition-(lettersOffset*(j+1));
            }
        }
    }

    public void Read()
    {
        if (isReading)
        {
            StopAllCoroutines();
            StartCoroutine(CloseLetterAnim());
            currentLettersData[currentLevel].GetUnread();
            isReading = false;
            PlaySound(clips[0],SoundType.Effects);
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(OpenLetterAnim());
            isReading = true;
            currentLettersData[currentLevel].GetRead();
            letterText.text = currentLettersData[currentLevel].content;
            letterAuthor.text = "De : " + "<color=#D70000><b>"+currentLettersData[currentLevel].author+"</b></color> ";
            PlaySound(clips[0],SoundType.Effects);
        }
        
    }
    public void Pin(int index)
    {
        if (currentLettersData[index].delivered == false)
        {
            if (currentLettersData[index].pinned)
            {
                pinnedCoordinates = null;
                currentLettersData[index].pinned = false;
                directionHelp.active = false;
                directionHelp.target = currentLettersData[index].destinationPosition;
                currentLettersData[index].GetUnpinned();
                PlaySound(clips[1],SoundType.Effects);
            }
            else
            {
                if (pinnedCoordinates != null)
                { 
                    letters[(pinnedCoordinates[1] * lettersByPage) + pinnedCoordinates[0]].GetComponent<LetterUI>().GetUnpinned();
                }
                pinnedCoordinates = new[] { index, currentPage };
                currentLettersData[index].pinned = true;
                directionHelp.active = true;
                directionHelp.target = currentLettersData[index].destinationPosition;
                currentLettersData[index].GetPinned();
                PlaySound(clips[1],SoundType.Effects);
            }
        }
        
    }

    public Vector3 ReturnPosOfLetter()
    {
        return currentLettersData[currentLevel].destinationPositionOnMap;
    }

    public Vector3 ReturnPosOfPinnedLetter()
    {
        return letters[(pinnedCoordinates[1]*lettersByPage)+pinnedCoordinates[0]].GetComponent<LetterUI>().destinationPositionOnMap;
    }

    public void ResetPin()
    {
        pinnedCoordinates = null;
        directionHelp.active = false;
        for (int i = 0; i < letters.Length; i++)
        {
            if (letters[i] != null)
            {
                LetterUI letterUI = letters[i].gameObject.GetComponent<LetterUI>();
                if (letterUI.pinned)
                {
                    letterUI.pinned = false;
                    break;
                }
            }
        }
    }

    IEnumerator OpenLetterAnim()
    { 
        readingSheet.anchoredPosition = new Vector3(-743f, -1000f, 0);
        float speed = 900;
        while (readingSheet.anchoredPosition.y < 17f)
        {
            Vector3 newPos = readingSheet.anchoredPosition;
            newPos.y += speed*Time.deltaTime;
            readingSheet.anchoredPosition = newPos;
            yield return null;
        }
    }

    IEnumerator CloseLetterAnim()
    { 
        float speed = 1500;
        while (readingSheet.anchoredPosition.y > -1000f)
        {
            Vector3 newPos = readingSheet.anchoredPosition;
            newPos.y -= speed*Time.deltaTime;
            readingSheet.anchoredPosition = newPos;
            yield return null;
        }
    }
    IEnumerator MoveSelector(Vector3 destination)
    {
        float speed = 1400f;
        RectTransform rectTransform = selectionPad;

        if (Vector3.Distance(destination, rectTransform.anchoredPosition) > 500)
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
}
