using System;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pnj : PlayerInteractable
{
    public PnjData pnjData;
    public string[] baseLines;
    private int lastLineSaid; 
    
    private DeliveryManager deliveryManager;
    private List<string> linesLeft;
    private bool justFinish = false;

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
    }

    public void GiveLetter(Letter letter)
    {
        Debug.Log("zour");
        for (int i = 0; i < letter.letterData.receivedText.Length; i++)
        {
            linesLeft.Add(letter.letterData.sendedText[i]);
        }
    }

    public override int GetPriority()
    {
        return 1;
    }

    public override void Interact()
    {
        deliveryManager.DeliveryCheck(this);
        if (linesLeft.Count == 0 && justFinish)
        {
            justFinish = false;
            deliveryManager.CreateValidLetters(this);
        }
        if (linesLeft.Count > 0)
        {
            Debug.Log(linesLeft[0]);
            linesLeft.RemoveAt(0);
            if (linesLeft.Count == 0)
            {
                justFinish = true;
            }
        }
        else if (linesLeft.Count <= 0)
        {
            int currentLine = Random.Range(0, baseLines.Length);
            while (currentLine == lastLineSaid && baseLines.Length > 1)
            {
                currentLine = Random.Range(0, baseLines.Length);
            }
            Debug.Log(baseLines[currentLine]);
            lastLineSaid = currentLine;
        }
    }

    public override bool CanInteract()
    {
        return true;
    }
}
