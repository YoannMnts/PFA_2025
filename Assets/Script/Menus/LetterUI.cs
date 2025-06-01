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
    
    [SerializeField] private Image pinnedImage,readImage,baseImage,bgImage, pinIndicator, readIndicator;
    [SerializeField] private Sprite alreadyDeliveredSprite;
    public TextMeshProUGUI destinationPerson;
    public Vector3 destinationPosition ;
    public Vector3 destinationPositionOnMap;
    public bool pinned = false;
    public string content ;
    public string author ;
    public LetterData data;
    public bool delivered = false;
    private void OnEnable()
    {
        pinnedImage.enabled = false;
        readImage.enabled = false;
        baseImage.enabled = true;
        pinIndicator.enabled = false;
        readIndicator.enabled = false;
    }

    public void GetPinned()
    {
        pinnedImage.enabled = true;
        pinIndicator.enabled = false;
    }

    public void GetUnpinned()
    {
        pinnedImage.enabled = false;
        pinIndicator.enabled = true;
    }

    public void GetRead()
    {
        readImage.enabled = true;
        baseImage.enabled = false;
        readIndicator.enabled = false;
    }

    public void GetUnread()
    {
        readImage.enabled = false;
        baseImage.enabled = true;
        readIndicator.enabled = true;
    }

    public void SetUp(LetterData data, bool alreadyDelivered)
    {
        this.data = data;
        if (alreadyDelivered)
        {
            bgImage.sprite = alreadyDeliveredSprite;
            delivered = true;
        }
        this.content = data.text;
        this.author = data.senderName;
        this.destinationPerson.text = "Pour : " + "<color=#D70000><b>"+data.receiver.name+"</b></color>" ;
        this.destinationPosition = data.receiver.position;
        this.destinationPositionOnMap = data.receiver.mapPosition;
    }

    public void GetHovered(bool cantRead)
    {
        if (cantRead == false)
        {
            readIndicator.enabled = true;
        }

        if (pinned == false && delivered == false)
        {
            pinIndicator.enabled = true;
        }
        
        
    }

    public void GetUnHovered()
    {
        pinIndicator.enabled = false;
        readIndicator.enabled = false;
    }
}
