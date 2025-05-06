using System;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

public class Pnj : PlayerInteractable
{
    public PnjData pnjData;
    
    private DeliveryManager deliveryManager;

    private void Awake()
    {
        deliveryManager = GameObject.FindGameObjectWithTag("DeliveryManager").GetComponent<DeliveryManager>();
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
        Debug.Log($"DeliverLetter {letter.letterData.name}", gameObject);
    }

    public override int GetPriority()
    {
        return 1;
    }

    public override void Interact()
    { 
        deliveryManager.DeliveryCheck(this);
    }

    public override bool CanInteract()
    {
        return true;
    }
}
