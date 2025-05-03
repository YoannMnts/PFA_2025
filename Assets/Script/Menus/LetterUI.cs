using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class LetterUI : MonoBehaviour
{
    
    [SerializeField] private Image pinnedImage;
    public TextMeshProUGUI destinationPerson;
    public Vector3 destinationPosition ;
    public Vector3 destinationPositionOnMap;
    public bool pinned = false;
    public string content = "olala";
    public string author = "leguman";
    private void OnEnable()
    {
        pinnedImage.enabled = false;
        destinationPosition = new Vector3(27, -115, 21);
        destinationPositionOnMap = new Vector3(10,10,10);
    }

    public void GetPinned()
    {
        pinnedImage.enabled = true;
    }

    public void GetUnpinned()
    {
        pinnedImage.enabled = false;
    }
}
