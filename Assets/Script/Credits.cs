using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RectTransform creditsObject;
    void Start()
    {
        StartCoroutine(Scrolling());
    }

    IEnumerator Scrolling()
    {
        float speed =40f;
        while (creditsObject.position.y <= 2000)
        {
            Vector3 pos = creditsObject.position;
            pos.y += speed * Time.deltaTime;
            creditsObject.position = pos;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(1);
    }
}
