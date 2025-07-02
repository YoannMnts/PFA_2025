using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class DeliveryManager : MonoBehaviour
{
    public List<Letter> ActiveLetter => activeLetter;
    public LetterData[] LetterDataTab => letterDataTab;
    public bool AlreadyInActiveLetter => alreadyInActiveLetters;
    


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
    [SerializeField] 
    public Pnj[] pnjsTab;
    [SerializeField] 
    private RewardParticles acornParticles, letterParticles, stampParticles;
    [SerializeField] 
    public Player player;
    [SerializeField] 
    private List<Letter> activeLetter;
    [SerializeField] 
    private LetterData saveLetterData;
    
    private Dictionary<PnjData, Pnj> pnjs;
    private bool alreadyInActiveLetters;
    
    public List<LetterData> completedLetters;
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
        StartCoroutine(ConstantCheck());
    }


    public Letter CreateLetter(LetterData letterData)
    {
        alreadyInActiveLetters = false;
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
                alreadyInActiveLetters = true;
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
                stampsPanel.UnlockStamp();
                inventoryManager.acornsCount += letter.letterData.glansGain;
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
                    
                }
                else
                {
                    if (pnj.pnjData == letterData.sender )
                    { 

                        Letter letter = CreateLetter(letterData); 
                        if (!alreadyInActiveLetters && letterData != saveLetterData) 
                        { 
                            activeLetter.Add(letter);
                            StartCoroutine(LetterNotif(letter, letter.sender));
                            if (letter.letterData.sender == pnj.pnjData) 
                            { 
                                pnj.GiveLetter(letter);
                            }
                        }
                    }
                }
            }
        }
        AboveHeadIndication();
    }



    IEnumerator Apparition(PnjData pnj1, PnjData pnj2)
    {
        if (pnj1 != null)
        {
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
                    distance = Vector3.Distance(pnjToAppear.transform.position, player.transform.position);
                    yield return null;
                }
                pnjToAppear.GetComponent<SpriteRenderer>().enabled = true;
                Collider2D[] colliders = pnjToAppear.GetComponents<BoxCollider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = true;
                }
                if (pnjToAppear.GetComponent<PolygonCollider2D>() != null)
                {
                    pnjToAppear.GetComponent<PolygonCollider2D>().enabled = true;
                }
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
                Collider2D[] colliders = pnjToDisapear.GetComponents<BoxCollider2D>();
                for (int i = 0; i < colliders.Length; i++)
                {
                    colliders[i].enabled = false;
                }
                if (pnjToDisapear.GetComponent<PolygonCollider2D>() != null)
                {
                    pnjToDisapear.GetComponent<PolygonCollider2D>().enabled = false;
                }
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
        if (letter.letterData.stampGain)
        {
            StartCoroutine(notification.ShowUpReward(letter.letterData.glansGain, true));
            stampParticles.Reward(1);
        }
        else
        {
            StartCoroutine(notification.ShowUpReward(letter.letterData.glansGain, false));
        }
        acornParticles.Reward(letter.letterData.glansGain);
        inventoryManager.OpenBagTemp();
    }

    void AboveHeadIndication()
    {
        for (int i = 0; i < pnjsTab.Length; i++)
        {
            Debug.Log("crasdh");
            pnjsTab[i].ActivatePopUp(false);
        }

        for (int i = 0; i < LetterDataTab.Length; i++)
        {
            if (LetterDataTab[i] == saveLetterData)
            {
                continue;
            }

            bool isAvailable = true;
            for (int j = 0; j < LetterDataTab[i].dependencies.Length; j++)
            {
                if (!completedLetters.Contains(LetterDataTab[i].dependencies[j]))
                {
                    isAvailable = false;
                }
            }

            if (isAvailable)
            {
                if (!completedLetters.Contains(LetterDataTab[i]))
                {
                    for (int j = 0; j < ActiveLetter.Count; j++)
                    {
                        if (ActiveLetter[j].letterData == LetterDataTab[i])
                        {
                            isAvailable = false;
                        }
                    }
                }
                else
                {
                    isAvailable = false;
                }
            }

            if (isAvailable)
            {
                for (int j = 0; j < pnjsTab.Length; j++)
                {
                    if (pnjsTab[j].pnjData == LetterDataTab[i].sender)
                    {
                        if (pnjsTab[j].GetComponent<SpriteRenderer>().enabled)
                        {
                            pnjsTab[j].ActivatePopUp(true);
                        }
                    }
                }
            }

            for (int k = 0; k < activeLetter.Count; k++)
            {
                for (int j = 0; j < pnjsTab.Length; j++)
                {
                    if (pnjsTab[j].pnjData == activeLetter[k].letterData.receiver)
                    {
                        pnjsTab[j].ActivatePopUp(true);
                    }
                }
            }
        }
    }

    IEnumerator ConstantCheck()
    {
        while (true)
        {
            AboveHeadIndication();
            yield return new WaitForSeconds(2f);
        }
    }
}