using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventoryManager : SoundObject
{
    [SerializeField] private GameObject inventoryTopPanel;
    [SerializeField] private GameObject panelSelector;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private GameObject acornsPanel;
    [SerializeField] private TextMeshProUGUI acornsText;
    [SerializeField] public GameObject closedBag, openBag;
    [SerializeField] public int acornsCount;
    [SerializeField] public Tutorial tutorial;
    
    private int currentPanel = 0;

    public override void Start()
    {
        base.Start();
        panelSelector.GetComponent<RectTransform>().anchoredPosition = panels[currentPanel].GetComponent<RectTransform>().anchoredPosition;
        foreach (GameObject panel in panels)
        {
            panel.GetComponent<Panel>().deliveryManager = deliveryManager;
            panel.GetComponent<Panel>().Close();
        }
        inventoryTopPanel.SetActive(false);
    }
    public void OpenTab(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (playerInput.currentActionMap.name == "Menu")
            {
                openBag.SetActive(false);
                closedBag.SetActive(true);
                playerInput.SwitchCurrentActionMap("GamePlay");
                PlaySound(clips[0],SoundType.Effects);
            }
            else
            {
                tutorial.Close();
                openBag.SetActive(true);
                closedBag.SetActive(false);
                playerInput.SwitchCurrentActionMap("Menu");
                PlaySound(clips[0],SoundType.Effects);
            }
            if (inventoryTopPanel.activeInHierarchy)
            {
                panels[currentPanel].GetComponent<Panel>().Close();
                currentPanel = 0;
                panelSelector.GetComponent<RectTransform>().anchoredPosition = panels[currentPanel].GetComponent<RectTransform>().anchoredPosition;
                inventoryTopPanel.SetActive(false);
            }
            else
            {
                inventoryTopPanel.SetActive(true);
                acornsText.text = acornsCount+" / 50";
                panels[0].GetComponent<Panel>().Open();
            }
        }
    }

    public void SwitchTabRight(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().Close();
            currentPanel += 1;
            if (currentPanel >= panels.Length)
            {
                currentPanel = 0;
            }
            StopAllCoroutines();
            StartCoroutine(MoveSelector(panels[currentPanel].GetComponent<RectTransform>().anchoredPosition));
            panels[currentPanel].GetComponent<Panel>().Open();
            PlaySound(clips[0],SoundType.Effects);
        }

        if (currentPanel == 1 || currentPanel == 2)
        {
            acornsPanel.SetActive(false);
        }
        else
        {
            acornsPanel.SetActive(true);
        }
    }
    public void SwitchTabLeft(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().Close();
            currentPanel -= 1;
            if (currentPanel <0)
            {
                currentPanel = panels.Length - 1;
            }
            StopAllCoroutines();
            StartCoroutine(MoveSelector(panels[currentPanel].GetComponent<RectTransform>().anchoredPosition));
            panels[currentPanel].GetComponent<Panel>().Open();
            PlaySound(clips[0],SoundType.Effects);
        }
        if (currentPanel == 1 || currentPanel == 2)
        {
            acornsPanel.SetActive(false);
        }
        else
        {
            acornsPanel.SetActive(true);
        }
    }

    IEnumerator MoveSelector(Vector3 destination)
    {
        float speed = 1400f;
        RectTransform rectTransform = panelSelector.GetComponent<RectTransform>();

        if (Vector3.Distance(destination, rectTransform.anchoredPosition) > 1000)
        {
            rectTransform.anchoredPosition = destination;
            
        }
        else
        {
            while (Vector3.Distance(destination, rectTransform.anchoredPosition) > 5)
            {
                rectTransform.anchoredPosition = Vector3.MoveTowards(rectTransform.anchoredPosition, destination, speed*Time.deltaTime);
                yield return null;
            }
            rectTransform.anchoredPosition = destination;
        }
    }

    public void ReceiveInputLeftDPad(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().LeftDPad();
        }
    }
    
    public void ReceiveInputRightDPad(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().RightDPad();
        }
        
    }
    
    public void ReceiveInputTopDPad(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().TopDPad();
        }

    }
    public void ReceiveInputBottomDPad(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().BottomDPad();
        }
    }
    public void ReceiveInputSouthButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().SouthButton();
        }
    }
    public void ReceiveInputEastButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().EastButton();
        }
    }

    public void ReceiveInputWestButton(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            panels[currentPanel].GetComponent<Panel>().WestButton();
        }
    }

    void OpenBag(bool open)
    {
        if (open)
        {
            openBag.SetActive(true);
            closedBag.SetActive(false);
        }
        else
        {
            closedBag.SetActive(true);
            openBag.SetActive(false);
        }
    }

    public void OpenBagTemp()
    {
        StartCoroutine(BagOpenFor());
    }

    IEnumerator BagOpenFor()
    {
        float timer = 0;
        while (timer < 4f)
        {
            openBag.SetActive(true);
            closedBag.SetActive(false);
            timer += Time.deltaTime;
            yield return null;
        }
        closedBag.SetActive(true);
        openBag.SetActive(false);
    }
}
