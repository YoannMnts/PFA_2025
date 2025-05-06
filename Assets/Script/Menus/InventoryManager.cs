using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryTopPanel;
    [SerializeField] private GameObject panelSelector;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private GameObject acornsPanel;
    [SerializeField] private TextMeshProUGUI acornsText;
    [SerializeField] public int acornsCount;
    
    private int currentPanel = 0;

    void Start()
    {
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
                playerInput.SwitchCurrentActionMap("GamePlay");
            }
            else
            {
                playerInput.SwitchCurrentActionMap("Menu");
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
            panelSelector.GetComponent<RectTransform>().anchoredPosition = panels[currentPanel].GetComponent<RectTransform>().anchoredPosition;
            panels[currentPanel].GetComponent<Panel>().Open();
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

            panelSelector.GetComponent<RectTransform>().anchoredPosition =
                panels[currentPanel].GetComponent<RectTransform>().anchoredPosition;
            panels[currentPanel].GetComponent<Panel>().Open();
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
}
