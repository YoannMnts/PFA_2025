using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TutoBlock : MonoBehaviour
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
