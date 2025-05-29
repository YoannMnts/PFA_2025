using System;
using System.Collections;
using UnityEngine;

public class TutoBlocking : MonoBehaviour
{
    [SerializeField] private DeliveryManager manager;
    private void Start()
    {
        StartCoroutine(Check());
    }

    IEnumerator Check()
    {
        while (manager.ActiveLetter.Count <= 0)
        {
            yield return null;
        }
        Destroy(gameObject);
    }
}
