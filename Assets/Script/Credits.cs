using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RectTransform creditsObject;
    void Start()
    {
        StartCoroutine(Scrolling());
    }

    public void Pass(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SceneManager.LoadScene(1);
        }
    }
    IEnumerator Scrolling()
    {
        float speed =40f;
        while (creditsObject.anchoredPosition.y <= 5000)
        {
            Vector3 pos = creditsObject.anchoredPosition;
            pos.y += speed * Time.deltaTime;
            creditsObject.anchoredPosition = pos;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
