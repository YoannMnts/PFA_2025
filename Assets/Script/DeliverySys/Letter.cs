using System;
using System.Collections.Generic;
using NUnit.Framework;
using Script.DeliverySys;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class Letter
{
    public LetterData letterData;
    public Pnj sender;
    public Pnj receiver;
    public DeliveryManager deliveryManager;
}
