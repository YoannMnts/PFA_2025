using System;
using Script.DeliverySys;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;
using Random = UnityEngine.Random;

public class LetterUI : MonoBehaviour
{
    
    [SerializeField] private Image pinnedImage,readImage,baseImage;
    public TextMeshProUGUI destinationPerson;
    public Vector3 destinationPosition ;
    public Vector3 destinationPositionOnMap;
    public bool pinned = false;
    public string content ;
    public string author ;
    public LetterData data;
    private void OnEnable()
    {
        pinnedImage.enabled = false;
        readImage.enabled = false;
        baseImage.enabled = true;
    }

    public void GetPinned()
    {
        pinnedImage.enabled = true;
    }

    public void GetUnpinned()
    {
        pinnedImage.enabled = false;
    }

    public void GetRead()
    {
        readImage.enabled = true;
        baseImage.enabled = false;
    }

    public void GetUnread()
    {
        readImage.enabled = false;
        baseImage.enabled = true;
    }

    public void SetUp(LetterData data)
    {
        this.data = data;
        this.content = data.text;
        this.author = data.senderName;
        this.destinationPerson.text = "To : " + "<color=#D70000><b>"+data.receiver.name+"</b></color>" ;
        this.destinationPosition = data.receiver.position;
        this.destinationPositionOnMap = data.receiver.mapPosition;
    }
}
