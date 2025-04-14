using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField]
    private LetterData[] letterData;
    
    private Dictionary<PnjData, PnjInteraction> pnjs = new Dictionary<PnjData, PnjInteraction>();

    [SerializeField] 
    private Player player;
    public void Add(PnjInteraction pnjInteraction)
    {
        pnjs.Add(pnjInteraction.pnjData, pnjInteraction);
    }

    public void Remove(PnjInteraction pnjInteraction)
    {
        pnjs.Remove(pnjInteraction.pnjData);
    }

    public void DeliveryCheck(PnjInteraction pnjInteraction)
    {
        if (player.Interaction.IsInteract)
        {
            for (int i = 0; i < letterData.Length; i++)
            {
                if (pnjInteraction.pnjData == letterData[i].receiver)
                {
                    Debug.Log("I received the delivery");
                }
                else if (pnjInteraction.pnjData == letterData[i].sender)
                {
                    GameObject.Find(letterData[i].receiver.name).GetComponent<PnjInteraction>().enabled = true;
                    Debug.Log("I am sending a delivery");
                }
            }
        }
    }
}
