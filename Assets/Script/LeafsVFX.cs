using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafsVFX : MonoBehaviour
{
    [SerializeField] private GameObject leafPrefab;
    private List<GameObject> leafZones;
    [SerializeField] private GameObject player;
    [SerializeField] private int nbHozi, nbVerti;

    private void Start()
    {
        leafZones = new List<GameObject>();
        for (int i = 0; i < nbHozi; i++)
        {
            for (int j = 0; j < nbVerti; j++)
            {
                GameObject leaf = Instantiate(leafPrefab, new Vector3(0,0,0), Quaternion.identity);
                leaf.transform.SetParent(transform);
                leaf.transform.localPosition = new Vector3(i*25, j*15, 0);
                leafZones.Add(leaf);
            }
        }
        StartCoroutine(DetectionOfPlayer());
    }

    IEnumerator DetectionOfPlayer()
    {
        Vector3 playerPos = player.transform.position;
        for (int i = 0; i < leafZones.Count; i++)
        {
            float distance = Vector3.Distance(player.transform.position, leafZones[i].transform.position);
            if (distance < 40f)
            {
                leafZones[i].SetActive(true);
            }
            else
            {
                leafZones[i].SetActive(false);
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(DetectionOfPlayer());
    }
}
