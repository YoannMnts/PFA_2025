using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class DialoguePad : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private TextMeshProUGUI textContainer;
    [SerializeField] private PlayerInput playerInput;

    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void SetUp(Sprite portrait, string text)
    {
        playerInput.SwitchCurrentActionMap("Interaction");
        this.gameObject.SetActive(true);
        portraitImage.sprite = portrait;
        textContainer.text = text;
    }

    public void Close()
    {
        playerInput.SwitchCurrentActionMap("GamePlay");
        this.gameObject.SetActive(false);
    }
}
