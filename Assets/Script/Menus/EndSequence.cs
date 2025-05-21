using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSequence : MonoBehaviour
{
    [SerializeField] private int countToWin;
    [SerializeField] private RectTransform endMessage;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private Image continueButton, blackScreen, goToLetterBoxIndication, finishLoad;
    [SerializeField] private GameObject finishButton;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private HazelLetterBox hazelLetterBox;
    private bool canInteract = false;
    private bool playerIn=false;
    private bool readyToCredits = false;

    private void Start()
    {
        goToLetterBoxIndication.gameObject.SetActive(false);
        endMessage.gameObject.SetActive(false);
        continueButton.gameObject.SetActive(false);
        blackScreen.gameObject.SetActive(false);
        finishButton.gameObject.SetActive(false);
        StartCoroutine(CheckWin());
    }

    IEnumerator CheckWin()
    {
        if (inventoryManager.acornsCount >= countToWin)
        {
            goToLetterBoxIndication.gameObject.SetActive(true);
            hazelLetterBox.canEnd = true;
            hazelLetterBox.aboveHeadIndication.GetComponent<SpriteRenderer>().enabled = true;
        }
        else
        {
            yield return new WaitForSeconds(7f);
            StartCoroutine(CheckWin());
        }
    }

    public IEnumerator EndingAppear()
    {
        while (playerInput.currentActionMap.name != "GamePlay")
        {
            yield return null;
        }
        playerInput.SwitchCurrentActionMap("ScenarisedSequence");
        endMessage.gameObject.SetActive(true);
        float speed = 400f;
        while (endMessage.anchoredPosition.y < 0)
        {
            Vector3 pos = endMessage.anchoredPosition;
            pos.y += speed * Time.deltaTime;
            endMessage.anchoredPosition = pos;
            yield return null;
        }
        float appearSpeed = 0.5f;
        continueButton.gameObject.SetActive(true);
        Color originColor = continueButton.color;
        continueButton.color = new Color(originColor.r, originColor.g, originColor.b, 0f);
        while (continueButton.color.a < 1)
        {
            Color color = continueButton.color;
            Color newColor = new Color(color.r, color.g, color.b, color.a+appearSpeed*Time.deltaTime);
            continueButton.color = newColor;
            yield return null;
        }
        canInteract = true;
    }

    public void Continue()
    {
        if (canInteract)
        {
            canInteract = false;
            StartCoroutine(EndingDisappear());
        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (playerIn && readyToCredits)
            {
                finishLoad.fillAmount += 0.22f;
                if (finishLoad.fillAmount >= 1)
                {
                    playerIn = false;
                    StopCoroutine(UnLoad());
                    StartCoroutine(SceneTransition());
                }
            }
        }
    }

    IEnumerator UnLoad()
    {
        while (finishLoad.gameObject.activeInHierarchy && playerIn)
        {
            finishLoad.fillAmount -= 0.2f * Time.deltaTime;
            yield return null;
        }
    }
    IEnumerator SceneTransition()
    {
        playerInput.SwitchCurrentActionMap("ScenarisedSequence");
        blackScreen.gameObject.SetActive(true);
        float appearSpeed = 0.5f;
        blackScreen.color = new Color(0, 0, 0, 0f);
        while (blackScreen.color.a<1)
        {
            Color color = blackScreen.color;
            Color newColor = new Color(color.r, color.g, color.b, color.a+appearSpeed*Time.deltaTime);
            blackScreen.color = newColor;
            yield return null;
        }
        SceneManager.LoadScene(2);
    }
    IEnumerator EndingDisappear()
    {
        float speed = 600f;
        float appearSpeed = 0.5f;
        hazelLetterBox.aboveHeadIndication.GetComponent<SpriteRenderer>().enabled = false;
        while (endMessage.anchoredPosition.y < 2000f || continueButton.color.a > 0)
        {
            Vector3 pos = endMessage.anchoredPosition;
            pos.y += speed * Time.deltaTime;
            endMessage.anchoredPosition = pos;
            Color color = continueButton.color;
            Color newColor = new Color(color.r, color.g, color.b, color.a-appearSpeed*Time.deltaTime);
            continueButton.color = newColor;
            yield return null;
        }
        readyToCredits = true;
        playerInput.SwitchCurrentActionMap("GamePlay");
        
        StartCoroutine(UnLoad());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = true;
            if (readyToCredits)
            {

                finishButton.gameObject.SetActive(true);                
                StartCoroutine(UnLoad());
                finishLoad.fillAmount = 0f;
            }
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIn = false;
            if (readyToCredits)
            {
                finishButton.gameObject.SetActive(false);
            }
        }
    }
}
