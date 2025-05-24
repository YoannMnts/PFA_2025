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
    [SerializeField] private Sprite portrait;
    [SerializeField] private SpriteRenderer popUp;
    private int lastLineSaid = 0; 
    
    private DeliveryManager deliveryManager;
    public List<string> linesLeft;
    private bool isTalking = false;

    private void Awake()
    {
        deliveryManager = GameObject.FindGameObjectWithTag("DeliveryManager").GetComponent<DeliveryManager>();
        linesLeft = new List<string>();
        popUp.enabled = false;
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
        for (int i = 0; i < letter.letterData.sendedText.Length; i++)
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
        if (linesLeft.Count == 0)
        {
            deliveryManager.CreateValidLetters(this);
        }
        if (linesLeft.Count > 0)
        {
            isTalking = true;
            deliveryManager.dialoguePad.SetUp(portrait, linesLeft[0]);
            linesLeft.RemoveAt(0);
        }
        else if (linesLeft.Count <= 0)
        {
            if (isTalking)
            {
                isTalking = false;
                deliveryManager.dialoguePad.Close();
            }
            else
            {
                isTalking = true;
                int currentLine = Random.Range(0, baseLines.Length);
                while (currentLine == lastLineSaid && baseLines.Length > 1) 
                { 
                    currentLine = Random.Range(0, baseLines.Length);
                } 
                
                deliveryManager.dialoguePad.SetUp(portrait, baseLines[currentLine]);
                lastLineSaid = currentLine;
            }
        }
        deliveryManager.CreateValidLetters(null);
            
    }

    public void ActivatePopUp(bool activate)
    {
        popUp.enabled = activate;
    }

    public override bool CanInteract()
    {
        return true;
    }
    
}
