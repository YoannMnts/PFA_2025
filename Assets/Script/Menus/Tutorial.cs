using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public GameObject tutoImage;

    void Start()
    {
        tutoImage.SetActive(false);
    }

    public void Open()
    {
        Debug.Log("Tutorial Open");
        if (tutoImage.activeInHierarchy)
        {
            tutoImage.SetActive(false);
        }
        else
        {
            tutoImage.SetActive(true);
        }
    }

    public void Close()
    {
        if (tutoImage.activeInHierarchy)
        {
            tutoImage.SetActive(false);
        }
    }
}
