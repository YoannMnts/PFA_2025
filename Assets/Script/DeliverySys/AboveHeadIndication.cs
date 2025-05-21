using System;
using System.Collections;
using UnityEngine;

public class AboveHeadIndication : MonoBehaviour
{
    [SerializeField] private GameObject indication;
    [SerializeField] private float baseY = 2.5f;
    private float endY = 3f;
    private float speed =1;

    private void Start()
    {
        baseY = indication.transform.localPosition.y;
        endY = indication.transform.localPosition.y+0.5f;
        StartCoroutine(GoUp());
    }

    IEnumerator GoUp()
    {
        while (indication.transform.localPosition.y < endY)
        {
            Vector3 pos = indication.transform.localPosition;
            indication.transform.localPosition = new Vector3(pos.x, pos.y + (speed*Time.deltaTime), pos.z);
            yield return null;
        }
        StartCoroutine(GoDown());
    }

    IEnumerator GoDown()
    {
        while (indication.transform.localPosition.y > baseY)
        {
            Vector3 pos = indication.transform.localPosition;
            indication.transform.localPosition = new Vector3(pos.x, pos.y - (speed*Time.deltaTime), pos.z);
            yield return null;
        }
        StartCoroutine(GoUp());
    }
}
