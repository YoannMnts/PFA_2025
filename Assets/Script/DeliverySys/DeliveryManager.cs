using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DeliveryManager : MonoBehaviour
{
    public List<Letter> ActiveLetter => activeLetter;


    [SerializeField] 
    private StampsPanel stampsPanel;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private LetterData[] letterDataTab;
    [SerializeField] 
    private Player player;
    
    [SerializeField] private List<Letter> activeLetter;
    private List<LetterData> completedLetters;
    private Dictionary<PnjData, Pnj> pnjs;
    private bool alreadyInActiveLetter;

    private void Awake()
    {
        pnjs = new Dictionary<PnjData, Pnj>();
        activeLetter = new List<Letter>();
        completedLetters = new List<LetterData>();
    }

    private void Start()
    {
        for (int i = 0; i < letterDataTab.Length; i++)
        {
            if (letterDataTab[i].dependencies.Length == 0)
            {
                Letter letter = CreateLetter(letterDataTab[i]);
                activeLetter.Add(letter);
            }
        }
    }

    private Letter CreateLetter(LetterData letterData)
    {
        alreadyInActiveLetter = false;
        Letter letter = new Letter()
        {
            letterData = letterData,
            deliveryManager = this,
            receiver = pnjs[letterData.receiver],
            sender = pnjs[letterData.sender],
        };
        foreach (var letterActive in activeLetter)
        {
            if (letterActive.letterData == letterData)
                alreadyInActiveLetter = true;
        }
        return letter;
    }

    public void AddPnj(Pnj pnj)
    {
        pnjs.Add(pnj.pnjData, pnj);
    }
    public void RemovePnj(Pnj pnj)
    {
        pnjs.Remove(pnj.pnjData);
    }

    public void DeliveryCheck(Pnj pnj)
    {
        List<Letter> copy = new List<Letter>(activeLetter);
        foreach (Letter letter in copy)
        {
            if (pnj == letter.receiver)
            {
                activeLetter.Remove(letter);
                pnj.DeliverLetter(letter);
                stampsPanel.UnlockStamp(letter.letterData.stampsGain);
                inventoryManager.acornsCount += letter.letterData.glansGain;
                completedLetters.Add(letter.letterData);
                player.AddGlans(letter.letterData.glansGain);
            }
        }
    }

    public void CreateValidLetters(Pnj pnj = null)
    {
        for (int i = 0; i < letterDataTab.Length; i++)
        {
            LetterData letterData = letterDataTab[i];
            if (completedLetters.Contains(letterData))
            {
                continue;
            }
            bool hasCompletedDependencies = true;
            for (int j = 0; j < letterData.dependencies.Length; j++)
            {
                LetterData dependency = letterData.dependencies[j];
                if (!completedLetters.Contains(dependency))
                {
                    hasCompletedDependencies = false;
                    break;
                }
            }
            if (hasCompletedDependencies)
            {
                Debug.Log("zavma");
                Letter letter = CreateLetter(letterData);
                if (!alreadyInActiveLetter)
                {
                    activeLetter.Add(letter);
                    if (letter.letterData.sender == pnj.pnjData)
                    {
                        pnj.GiveLetter(letter);
                    }
                }
                    
            }
        }
    }
}
