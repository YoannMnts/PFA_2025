using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using Script.DeliverySys;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuBackgroun : MonoBehaviour
{
    [SerializeField] private Sprite[] stamps;
    [SerializeField] private GameObject stamp;
    [SerializeField] private float speed;
    [SerializeField] private float baseRate;
    [SerializeField] private Vector3 Spawning;
    private float elapsed;
    private float timer;
    private float rate;

    private void Start()
    {
        rate = 1/baseRate;
        StartCoroutine(Scroll());
    }

    IEnumerator Scroll()
    {
        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= rate)
            {
                elapsed = 0;
                rate = 1/(baseRate+Random.Range(-0.5f,0.5f));
                GameObject newStamp = Instantiate(stamp, new Vector3(Spawning.x, Spawning.y+Random.Range(-4f,4f),0), Quaternion.identity);
                newStamp.transform.SetParent(transform);
                newStamp.GetComponent<SpriteRenderer>().sprite = stamps[Random.Range(0,stamps.Length)];
            }
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                Transform currentStamp = gameObject.transform.GetChild(i);
                currentStamp.position = new Vector3(currentStamp.position.x-(speed*Time.deltaTime), currentStamp.position.y, currentStamp.position.z); 
                if (currentStamp.position.x <= -20)
                { 
                    Destroy(currentStamp.gameObject);
                }
            }
            yield return null;
        }
    }
}
