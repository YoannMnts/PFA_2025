using System;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;

public class PnjInteraction : MonoBehaviour
{
    public PnjData pnjData;
    
    private DeliveryManager deliveryManager;

    private void Awake()
    {
        deliveryManager = GameObject.FindGameObjectWithTag("DeliveryManager").GetComponent<DeliveryManager>();
    }

    private void OnEnable()
    {
        deliveryManager.Add(this);
    }

    private void OnDisable()
    {
        deliveryManager.Remove(this);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Player>().Inputs.SwitchCurrentActionMap("PnjInteraction");
            deliveryManager.DeliveryCheck(this);
        }
    }
    
     private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponentInParent<Player>().Inputs.SwitchCurrentActionMap("GamePlay");
        }
    }
}
