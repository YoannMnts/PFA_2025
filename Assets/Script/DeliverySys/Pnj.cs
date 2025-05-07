using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

public class Pnj : PlayerInteractable
{
    public PnjData pnjData;
    public string baseLine;
    
    private DeliveryManager deliveryManager;
    private List<string> linesLeft;
    private bool justDoneTalking = false;

    private void Awake()
    {
        deliveryManager = GameObject.FindGameObjectWithTag("DeliveryManager").GetComponent<DeliveryManager>();
        linesLeft = new List<string>();
    }

    private void OnEnable()
    {
        deliveryManager.AddPnj(this);
    }

    private void OnDisable()
    {
        deliveryManager.RemovePnj(this);
    }

    public void DeliverLetter(Letter letter)
    {
        for (int i = 0; i < letter.letterData.receivedText.Length; i++)
        {
            linesLeft.Add(letter.letterData.receivedText[i]);
        }
        if (linesLeft.Count > 0)
        {
            Debug.Log(linesLeft[0]);
            linesLeft.RemoveAt(0);
        }
    }

    public void GiveLetter(Letter letter)
    {
        for (int i = 0; i < letter.letterData.receivedText.Length; i++)
        {
            linesLeft.Add(letter.letterData.sendedText[i]);
        }
        if (linesLeft.Count > 0)
        {
            Debug.Log(linesLeft[0]);
            linesLeft.RemoveAt(0);
        }
    }

    public override int GetPriority()
    {
        return 1;
    }

    public override void Interact()
    {
        deliveryManager.DeliveryCheck(this);
        if (linesLeft.Count > 0)
        {
            Debug.Log(linesLeft[0]);
            linesLeft.RemoveAt(0);
            if (linesLeft.Count == 0)
            {
                justDoneTalking = true;
            }
        }
        if (justDoneTalking)
        {
            justDoneTalking = false;
            deliveryManager.CreateValidLetters(this);
        }
        else
        {
            Debug.Log(baseLine);
        }
    }

    public override bool CanInteract()
    {
        return true;
    }
}
