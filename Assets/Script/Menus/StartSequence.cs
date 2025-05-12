using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartSequence : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private Image blackBackground, fillBar, selfBlackBackground;
    [SerializeField] private RectTransform introLetter;
    [SerializeField] private GameObject continueIndication;
    private CinemachineCamera introCam;
    private PlayerInput playerInputOther;
    private bool canInteract = false;

    void Start()
    {
        blackBackground.gameObject.SetActive(false);
        selfBlackBackground.gameObject.SetActive(false);
        fillBar.gameObject.SetActive(false);
        continueIndication.SetActive(false);
        introLetter.gameObject.SetActive(false);
        
    }
    public IEnumerator Play()
    {
        playerInput.SwitchCurrentActionMap("ScenarisedSequence");
        blackBackground.gameObject.SetActive(true);
        StartCoroutine(Appear(blackBackground,2f));
        yield return new WaitForSeconds(3f);
        StartCoroutine(DisplayIntroLetter());
    }

    IEnumerator Appear(Image image, float time)
    {
        float speed = 1 / time;
        Color originColor = image.color;
        image.color = new Color(originColor.r, originColor.g, originColor.b, 0f);
        while (image.color.a < 1)
        {
            Color color = image.color;
            Color newColor = new Color(color.r, color.g, color.b, color.a+speed*Time.deltaTime);
            image.color = newColor;
            yield return null;
        }
    }
    IEnumerator Disapear(Image image, float time)
    {
        float speed = 1 / time;
        Color originColor = image.color;
        image.color = new Color(originColor.r, originColor.g, originColor.b, 1f);
        while (image.color.a>0)
        {
            Color color = image.color;
            Color newColor = new Color(color.r, color.g, color.b, color.a-speed*Time.deltaTime);
            image.color = newColor;
            yield return null;
        }
    }
    IEnumerator DisplayIntroLetter()
    {
        introLetter.gameObject.SetActive(true);
        float speed = 200f;
        while (introLetter.anchoredPosition.y < 0)
        {
            Vector3 pos = introLetter.anchoredPosition;
            pos.y += speed * Time.deltaTime;
            introLetter.anchoredPosition = pos;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        canInteract = true;
        continueIndication.SetActive(true);
        StartCoroutine(Appear(continueIndication.GetComponent<Image>(), 2f));
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            if (canInteract)
            {
                StartCoroutine(Continue());
            }
        }
    }

    IEnumerator Continue()
    {
        canInteract = false;
        StartCoroutine(Disapear(continueIndication.GetComponent<Image>(), 2f));
        continueIndication.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        StartCoroutine(Disapear(introLetter.GetComponent<Image>(), 2f));
        yield return new WaitForSeconds(2f);
        fillBar.gameObject.SetActive(true);
        fillBar.fillAmount = 0;
        AsyncOperation loading = SceneManager.LoadSceneAsync(0);
        while (SceneManager.GetSceneAt(0).isLoaded == false)
        {
            fillBar.fillAmount = loading.progress;
            yield return null;
        }

        StartCoroutine(EnterInScene());
    }

    IEnumerator EnterInScene()
    {
        selfBlackBackground.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(Disapear(selfBlackBackground, 3f));
        playerInputOther = FindObjectOfType<PlayerInput>();
        if (playerInputOther != null)
        {
            playerInputOther.SwitchCurrentActionMap("ScenarisedSequence");
        }
        introCam = FindObjectOfType<CinemachineCamera>();
        if (introCam != null) 
        { 
            Debug.Log(introCam); 
            StartCoroutine(CameraDezoom(3f));
        }
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryManager.closedBag.SetActive(false);
            inventoryManager.openBag.SetActive(false);
        }
        yield return new WaitForSeconds(7f);
        inventoryManager.closedBag.SetActive(true);
        if (playerInputOther != null)
        {
            playerInputOther.SwitchCurrentActionMap("GamePlay");
        }
        
    }

    IEnumerator CameraDezoom(float time)
    {
        introCam.Lens.OrthographicSize = 2;
        yield return new WaitForSeconds(3);
        float speed = 7/time;
        while (introCam.Lens.OrthographicSize < 7)
        {
            introCam.Lens.OrthographicSize += speed * Time.deltaTime;
            yield return null;
        }
    }
}
