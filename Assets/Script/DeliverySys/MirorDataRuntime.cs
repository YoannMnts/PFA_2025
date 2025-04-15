using System;
using System.Collections.Generic;
using NUnit.Framework;
using Script.DeliverySys;
using UnityEngine;

public class MirorDataRuntime : MonoBehaviour
{
    public LetterData letterData;
    private DeliveryManager deliveryManager;

    private void Awake()
    {
        deliveryManager = GameObject.FindGameObjectWithTag("DeliveryManager").GetComponent<DeliveryManager>();
    }

    private void OnEnable()
    {
        deliveryManager.addLetter(this);
    }

    private void OnDisable()
    {
        deliveryManager.removeLetter(this);
    }
}
