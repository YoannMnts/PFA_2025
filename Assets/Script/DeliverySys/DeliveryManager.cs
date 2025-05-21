using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DeliveryManager : MonoBehaviour
{
    public List<Letter> ActiveLetter => activeLetter;


    [SerializeField] 
    private StampsPanel stampsPanel;
    [SerializeField]
    private LettersPanel lettersPanel;
    [SerializeField]
    private QuitPanel quitPanel;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private Notification notification;
    [SerializeField]
    private LetterData[] letterDataTab;
    [SerializeField] private Pnj[] pnjsTab;
    [SerializeField] private RewardParticles acornParticles, letterParticles, stampParticles;
    [SerializeField] 
    private Player player;
    
    [SerializeField] private List<Letter> activeLetter;
    public List<LetterData> completedLetters;
    private Dictionary<PnjData, Pnj> pnjs;
    private bool alreadyInActiveLetter;
    public  DialoguePad dialoguePad;

    private void Awake()
    {
        pnjs = new Dictionary<PnjData, Pnj>();
        activeLetter = new List<Letter>();
        completedLetters = new List<LetterData>();
    }

    private void Start()
    {
        CreateValidLetters(null);
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
        player.gameObject.GetComponent<PlayerMovement>().Freeze();
        List<Letter> copy = new List<Letter>(activeLetter);
        foreach (Letter letter in copy)
        {
            if (pnj == letter.receiver && activeLetter.Contains(letter))
            {
                lettersPanel.ResetPin();
                activeLetter.Remove(letter);
                pnj.DeliverLetter(letter);
                stampsPanel.UnlockStamp(letter.letterData.stampsGain);
                inventoryManager.acornsCount += letter.letterData.glansGain;
                quitPanel.lettersCount += 1;
                StartCoroutine(Apparition(letter.letterData.appearingCharacter, letter.letterData.disappearingCharacter));
                StartCoroutine(RewardNotif(letter, letter.receiver));
                completedLetters.Add(letter.letterData);
                player.AddGlans(letter.letterData.glansGain);
            }
        }
    }

    public void CreateValidLetters(Pnj pnj)
    {
        for (int i = 0; i < letterDataTab.Length; i++)
        {
            LetterData letterData = letterDataTab[i];
            if (completedLetters.Contains(letterData))
            {
                continue;
            }
            bool hasCompletedDependencies = true;
            for (int j = 0; j < pnjsTab.Length; j++)
            {
                pnjsTab[i].ActivatePopUp(false);
            }
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
                if (pnj == null)
                {
                    CheckPopUp(letterData);
                }
                else
                {
                    if (pnj.pnjData == letterData.sender)
                    { 
                        Letter letter = CreateLetter(letterData); 
                        if (!alreadyInActiveLetter) 
                        { 
                            activeLetter.Add(letter);
                            StartCoroutine(LetterNotif(letter, letter.sender));
                            if (letter.letterData.sender == pnj.pnjData) 
                            { 
                                pnj.GiveLetter(letter);
                            }
                        }
                    }
                    else 
                    { 
                        CheckPopUp(letterData);
                    }
                }
            }
            for (int k = 0; k < ActiveLetter.Count; k++)
            {
                for (int j = 0; j < pnjsTab.Length; j++)
                {
                    if (pnjsTab[j].pnjData == ActiveLetter[k].letterData.receiver)
                    {
                        pnjsTab[j].ActivatePopUp(true);
                    }
                    else
                    {
                        pnjsTab[j].ActivatePopUp(false);
                    }
                }
            }
        }
    }

    private void CheckPopUp(LetterData letterData)
    {
        for (int i = 0; i < pnjsTab.Length; i++)
        {
            if (pnjsTab[i].pnjData == letterData.sender )
            {
                pnjsTab[i].ActivatePopUp(true);
                for (int j = 0; j < activeLetter.Count; j++)
                {
                    if (activeLetter[j].letterData == letterData)
                    {
                        pnjsTab[i].ActivatePopUp(false);
                    }
                }
            }
            else
            {
                pnjsTab[i].ActivatePopUp(false);
            }
        }
        
        for (int i = 0; i < ActiveLetter.Count; i++)
        {
            for (int j = 0; j < pnjsTab.Length; j++)
            {
                if (pnjsTab[j].pnjData == ActiveLetter[i].letterData.receiver)
                {
                    pnjsTab[j].ActivatePopUp(true);
                }
                else
                {
                    pnjsTab[j].ActivatePopUp(false);
                }
            }
        }
    }

    IEnumerator Apparition(PnjData pnj1, PnjData pnj2)
    {
        if (pnj1 != null)
        {
            Debug.Log("tss");
            GameObject pnjToAppear = null;
            for (int i = 0; i < pnjsTab.Length; i++)
            {
                if (pnjsTab[i].pnjData == pnj1)
                {
                    pnjToAppear = pnjsTab[i].gameObject;
                }
            }

            if (pnjToAppear != null)
            {
                float distance = Vector3.Distance(pnjToAppear.transform.position, player.transform.position);
                while (distance < 15)
                {
                    yield return null;
                }
                Debug.Log(pnjToAppear.name);
                pnjToAppear.GetComponent<SpriteRenderer>().enabled = true;
                pnjToAppear.GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (pnj2 != null)
        {
            GameObject pnjToDisapear= null;
            for (int i = 0; i < pnjsTab.Length; i++)
            {
                if (pnjsTab[i].pnjData == pnj2)
                {
                    pnjToDisapear = pnjsTab[i].gameObject;
                }
            }

            if (pnjToDisapear != null)
            {
                float distance = Vector3.Distance(pnjToDisapear.transform.position, player.transform.position);
                while (distance < 15)
                {
                    yield return null;
                }
                pnjToDisapear.GetComponent<SpriteRenderer>().enabled = false;
                pnjToDisapear.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
    }

    IEnumerator LetterNotif(Letter letter, Pnj pnj)
    {
        yield return null;
        while (pnj.linesLeft.Count>0)
        {
            yield return null;
        }
        StartCoroutine(notification.ShowUpLetter(letter.letterData.receiver.name.ToString()));
        letterParticles.Reward(1);
        inventoryManager.OpenBagTemp();
    }

    IEnumerator RewardNotif(Letter letter, Pnj pnj)
    {
        yield return null;
        while (pnj.linesLeft.Count > 0)
        {
            yield return null;
        }
        if (letter.letterData.stampsGain >= 0)
        {
            StartCoroutine(notification.ShowUpReward(letter.letterData.glansGain, true));
        }
        else
        {
            StartCoroutine(notification.ShowUpReward(letter.letterData.glansGain, false));
        }
        acornParticles.Reward(letter.letterData.glansGain);
        stampParticles.Reward(1);
        inventoryManager.OpenBagTemp();
    }
}