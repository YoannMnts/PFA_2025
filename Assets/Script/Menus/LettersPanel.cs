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
    [SerializeField] private GameObject readingSheet;
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private TextMeshProUGUI letterAuthor;
    private GameObject[] letters;
    private GameObject[] currentLetters;
    private LetterUI[] currentLettersData;
    [SerializeField] private RectTransform selectionPad;
    [SerializeField] Vector3 lettersBasePosition;
    [SerializeField] Vector3 lettersOffset;
    [SerializeField] int lettersByPage ;
    private int lettersCount = 16;
    private int currentLevel;
    private int currentPage;
    public int[] pinnedCoordinates;
    private bool isReading;

    public override void Close()
    {
        readingSheet.SetActive(false);
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
        readingSheet.SetActive(false);
        isReading = false;
        currentLevel = 0;
        currentPage = 0;
        letters = new GameObject[lettersCount];
        currentLetters = new GameObject[lettersByPage];
        for (int i = 0; i < lettersCount; i++)
        {
            GameObject letter = Instantiate(letterTemplate, panel.transform);
            letters[i] = letter;
            letter.SetActive(false);
        }
        DisplayLetters();
        selectionPad.position = letters[0].GetComponent<RectTransform>().position;
    }

    public override void Awake()
    {
        base.Awake();
        currentLevel = 0;
        currentPage = 0;
        
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
            selectionPad.position = letters[currentLevel + (currentPage*lettersByPage)].GetComponent<RectTransform>().position;
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
             selectionPad.position = letters[currentLevel+ (currentPage*lettersByPage)].GetComponent<RectTransform>().position;
        }
       
    }

    public override void SouthButton()
    {
        base.SouthButton();
        Pin(currentLevel);
    }

    public override void WestButton()
    {
        base.WestButton();  
        Read();
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
        if (readingSheet.gameObject.activeInHierarchy)
        {
            readingSheet.SetActive(false);
            isReading = false;
        }
        else
        {
            readingSheet.SetActive(true);
            isReading = true;
            letterText.text = currentLettersData[currentLevel].content;
            letterAuthor.text = "From : " + currentLettersData[currentLevel].author;
        }
        
    }
    public void Pin(int index)
    {
        if (currentLettersData[index].pinned)
        {
            pinnedCoordinates = null;
            currentLettersData[index].pinned = false;
            directionHelp.active = false;
            directionHelp.target = currentLettersData[index].destinationPosition;
            currentLettersData[index].GetUnpinned();
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
}
