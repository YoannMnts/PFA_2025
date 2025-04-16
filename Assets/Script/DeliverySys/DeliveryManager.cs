using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DeliveryManager : MonoBehaviour
{
    [SerializeField]
    private LetterData[] letterDataTab;
    [SerializeField] 
    private Player player;
    
    private List<Letter> activeLetter;
    private List<LetterData> completedLetters;
    private Dictionary<PnjData, Pnj> pnjs;

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
        Letter letter = new Letter()
        {
            letterData = letterData,
            deliveryManager = this,
            receiver = pnjs[letterData.receiver],
            sender = pnjs[letterData.sender],
        };
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
                completedLetters.Add(letter.letterData);
            }
        }
        CreateValidLetters();
    }

    private void CreateValidLetters()
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
                Letter letter = CreateLetter(letterData);
                activeLetter.Add(letter);
            }
        }
    }
}
