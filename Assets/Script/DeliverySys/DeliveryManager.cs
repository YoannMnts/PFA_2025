using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    
    
    [SerializeField]
    private LetterData[] letterDataTab;
    private Dictionary<PnjData, PnjInteraction> pnjs = new Dictionary<PnjData, PnjInteraction>();
    [SerializeField] 
    private Player player;
    private List<Letter> activeLetter = new List<Letter>();
    public void AddPnj(PnjInteraction pnjInteraction)
    {
        pnjs.Add(pnjInteraction.pnjData, pnjInteraction);
    }
    public void RemovePnj(PnjInteraction pnjInteraction)
    {
        pnjs.Remove(pnjInteraction.pnjData);
    }
    public void addLetter(Letter mirorData)
    {
        activeLetter.Add(mirorData);
    }
    public void removeLetter(Letter mirorData)
    {
        activeLetter.Remove(mirorData);
    }

    public void DeliveryCheck(PnjInteraction pnjInteraction)
    {
        if (player.Interaction.IsInteract)
        {
            for (int i = 0; i < activeLetter.Count; i++)
            {
                if (pnjInteraction.pnjData == activeLetter[i].letterData.receiver)
                {
                    Debug.Log("I received the delivery");
                    activeLetter[i].nextLetter.enabled = true;
                    activeLetter[i].enabled = false;
                }
                else if (pnjInteraction.pnjData == activeLetter[i].letterData.sender)
                {
                    GameObject.Find(activeLetter[i].letterData.receiver.name).GetComponent<PnjInteraction>().enabled = true;
                    Debug.Log("I am sending a delivery");
                }
            }
        }
    }
}
