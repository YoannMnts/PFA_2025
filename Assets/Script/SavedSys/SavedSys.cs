using System;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class SavedSys : MonoBehaviour
{
    public bool AlreadySaved => alreadySaved;
    
    [SerializeField]
    private GameObject playerGo;
    [SerializeField]
    private DeliveryManager deliveryManager;
    [SerializeField]
    private InventoryManager inventoryManager;
    [SerializeField]
    private GameObject pnjGo;
    [SerializeField]
    private StampsPanel stampsPanel;
    
    private bool alreadySaved;

    void Start()
    { 
        alreadySaved = PlayerPrefs.GetInt("alreadySaved") == 1;
        if (alreadySaved)
        {
            //PlayerPrefs.DeleteAll();
            Load();
        }
    }

    public void Saved()
    {
        PlayerPrefs.SetInt("Acorns", inventoryManager.acornsCount);
        PlayerPrefs.SetInt("alreadySaved", 1);
        
        PlayerPosSaved();
        CompletedLetterSaved();
        ActiveLetterSaved();
        PnjSaved();
        StampsSaved();
        Debug.Log("saved");
    }
    
    public void Load()
    {
        inventoryManager.acornsCount = PlayerPrefs.GetInt("Acorns");
        
        PlayerPosLoad();
        CompletedLetterLoad();
        ActiveLetterLoad();
        PnjLoad();
        StampsLoad();
        Debug.Log("Loaded");
    }


    #region SavedMethods
    
    private void PlayerPosSaved()
    {
        PlayerPrefs.SetFloat("PlayerPosX", playerGo.transform.position.x);
        PlayerPrefs.SetFloat("PlayerPosY", playerGo.transform.position.y);
    }
    private void StampsSaved()
    {
        for (int i = 0; i < stampsPanel.UnlockedStamps.Length; i++)
        {
            PlayerPrefs.SetInt("UnlockedStamps" + i, stampsPanel.UnlockedStamps[i] == true ? 1 : 0);
        }
    }

    private void PnjSaved()
    {
        foreach (var children in pnjGo.GetComponentsInChildren<SpriteRenderer>())
        {
            PlayerPrefs.SetInt(children.name + "SpriteRenderer", children.GetComponent<SpriteRenderer>().enabled ? 1 : 0);
        }

        foreach (var children in pnjGo.GetComponentsInChildren<BoxCollider2D>())
        {
            PlayerPrefs.SetInt(children.name + "BoxCollider2D", children.GetComponent<BoxCollider2D>().enabled ? 1 : 0);
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
                    PlayerPrefs.SetInt("CompletedLetter" + deliveryManager.LetterDataTab[j].ToString(), j);
                }
            }
        }
    }

    private void ActiveLetterSaved()
    {
        for (int i = 0; i < deliveryManager.ActiveLetter.Count; i++)
        {
            for (int j = 0; j < deliveryManager.LetterDataTab.Length; j++)
            {
                if (deliveryManager.ActiveLetter[i].letterData == deliveryManager.LetterDataTab[j])
                {
                    PlayerPrefs.SetInt("ActiveLetter" + deliveryManager.LetterDataTab[j].ToString(), j);
                }
            }
        }
    }
    
    #endregion

    #region LoadMethods

    private void StampsLoad()
    {
        Debug.Log(stampsPanel.UnlockedStamps);
        for (int i = 0; i < stampsPanel.UnlockedStamps.Length; i++)
        {
            stampsPanel.UnlockedStamps[i] = PlayerPrefs.GetInt("UnlockedStamps" + i) == 1;
        }
    }
    private void PlayerPosLoad()
    {
        var vector3 = playerGo.transform.position;
        vector3.x = PlayerPrefs.GetFloat("PlayerPosX", -63);
        vector3.y = PlayerPrefs.GetFloat("PlayerPosY", -75);
        playerGo.transform.position = vector3;
    }
    private void PnjLoad()
    {
        foreach (var children in pnjGo.GetComponentsInChildren<SpriteRenderer>())
        {
            children.GetComponent<SpriteRenderer>().enabled = PlayerPrefs.GetInt(children.name + "SpriteRenderer") == 1;
        }
        foreach (var children in pnjGo.GetComponentsInChildren<BoxCollider2D>())
        {
            children.GetComponent<BoxCollider2D>().enabled = PlayerPrefs.GetInt(children.name + "BoxCollider2D") == 1;
        }
    }

    private void CompletedLetterLoad()
    {
        for (int i = 0; i < deliveryManager.LetterDataTab.Length; i++)
        {
            if (deliveryManager.completedLetters.Contains(deliveryManager.LetterDataTab[PlayerPrefs.GetInt("CompletedLetter" + deliveryManager.LetterDataTab[i].ToString())]) || PlayerPrefs.GetInt("CompletedLetter" + deliveryManager.LetterDataTab[i].ToString()) == 0)
            {
                continue;
            }
            deliveryManager.completedLetters.Add(deliveryManager.LetterDataTab[PlayerPrefs.GetInt("CompletedLetter" + deliveryManager.LetterDataTab[i].ToString())]);
        }
    }

    private void ActiveLetterLoad()
    {
        for (int i = 0; i < deliveryManager.LetterDataTab.Length; i++)
        {
            Letter letter = deliveryManager.CreateLetter(deliveryManager.LetterDataTab[PlayerPrefs.GetInt("ActiveLetter" + deliveryManager.LetterDataTab[i].ToString())]);
            if (deliveryManager.AlreadyInActiveLetter || letter.letterData == deliveryManager.LetterDataTab[0] || deliveryManager.completedLetters.Contains(letter.letterData))
            {
                continue;
            }
            deliveryManager.ActiveLetter.Add(letter);
        }
    }
    
    #endregion
}
