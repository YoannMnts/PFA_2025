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
    public string content ;
    public string author ;
    private void OnEnable()
    {
        pinnedImage.enabled = false;
    }

    public void GetPinned()
    {
        pinnedImage.enabled = true;
    }

    public void GetUnpinned()
    {
        pinnedImage.enabled = false;
    }

    public void SetUp(string content, string author, string destinationPerson, Vector3 destinationPosition, Vector3 destinationPositionOnMap)
    {
        this.content = content;
        this.author = author;
        this.destinationPerson.text = "To : " + destinationPerson;
        this.destinationPosition = destinationPosition;
        this.destinationPositionOnMap = destinationPositionOnMap;
    }
}
