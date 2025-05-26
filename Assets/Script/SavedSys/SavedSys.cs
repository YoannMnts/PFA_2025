using System;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class SavedSys : MonoBehaviour
{
    [SerializeField]
    private GameObject playerGO;
    [SerializeField]
    private DeliveryManager deliveryManager;

    [SerializeField]
    private GameObject test;
    
    void Start()
    {
        PlayerPrefs.DeleteAll();
        //Load();
    }

    public void Saved()
    {
        PlayerPrefs.SetFloat("PlayerPosX", playerGO.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerGO.transform.position.y);
        PlayerPrefs.SetInt("Acorns", 9);
        CompletedLetterSaved();
        PnjSaved();
        Debug.Log("saved");
    }
    public void Load()
    {
        var vector3 = playerGO.transform.position;
        vector3.x = PlayerPrefs.GetFloat("PlayerPosX", -63);
        vector3.y = PlayerPrefs.GetFloat("PlayerPosY", -75);
        playerGO.transform.position = vector3;
        PlayerPrefs.GetInt("Acorns");
        CompletedLetterLoad();
        PnjLoad();
    }


    private void PnjSaved()
    {
        foreach (var children in test.GetComponentsInChildren<Transform>())
        {
            PlayerPrefs.SetFloat(children.name + "x", children.position.x);
            PlayerPrefs.SetFloat(children.name + "y", children.position.y);
        }
    }

    private void CompletedLetterSaved()
    {
        for (int i = 0; i < deliveryManager.completedLetters.Count; i++)
        {
            for (int j = 0; j < deliveryManager.LetterDataTab.Length; j++)
            {
                if (deliveryManager.completedLetters[i] == deliveryManager.LetterDataTab[j])
                {
                    PlayerPrefs.SetInt(deliveryManager.LetterDataTab[j].ToString(), j);
                }
            }
        }
    }

    private void PnjLoad()
    {
        foreach (var children in test.GetComponentsInChildren<Transform>())
        {
            var vector3 = Vector3.zero;
            vector3.x = PlayerPrefs.GetFloat(children.name + "x");
            vector3.y = PlayerPrefs.GetFloat(children.name + "y");
            children.position = vector3;
        }
    }

    private void CompletedLetterLoad()
    {
        for (int i = 0; i < deliveryManager.LetterDataTab.Length; i++)
        {
            if (deliveryManager.completedLetters.Contains(deliveryManager.LetterDataTab[PlayerPrefs.GetInt(deliveryManager.LetterDataTab[i].ToString())]) || PlayerPrefs.GetInt(deliveryManager.LetterDataTab[i].ToString()) == 0)
            {
                continue;
            }
            deliveryManager.completedLetters.Add(deliveryManager.LetterDataTab[PlayerPrefs.GetInt(deliveryManager.LetterDataTab[i].ToString())]);
        }
    }
}
