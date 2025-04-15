using System;
using System.Collections.Generic;
using NUnit.Framework;
using Script.DeliverySys;
using UnityEngine;
using UnityEngine.Serialization;

public class Letter : MonoBehaviour
{
    public LetterData letterData;
    [SerializeField]
    public Letter nextLetter;
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
