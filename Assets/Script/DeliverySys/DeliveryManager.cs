using System;
using System.Collections.Generic;
using Script.DeliverySys;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{
    [SerializeField]
    private LetterData[] letterData;

    private Dictionary<PnjData, PnjInteraction> pnjs;

    private void Awake()
    {
        pnjs = new Dictionary<PnjData, PnjInteraction>();
    }

    public void Add(PnjInteraction pnjInteraction)
    {
        pnjs.Add(pnjInteraction.pnjData, pnjInteraction);
    }

    public void Remove(PnjInteraction pnjInteraction)
    {
        pnjs.Remove(pnjInteraction.pnjData);
    }

    public void ReceiverCheck(PnjInteraction pnjInteraction)
    {
        for (int i = 0; i < letterData.Length; i++)
        {
            if (pnjInteraction.pnjData == letterData[i].receiver)
            {
                Debug.Log("I'm a receiver");
            }
        }
    }
}
