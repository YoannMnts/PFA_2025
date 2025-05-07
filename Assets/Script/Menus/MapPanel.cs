using UnityEngine;
using UnityEngine.UI;

public class MapPanel : Panel
{
    [SerializeField] private LettersPanel lettersPanel;
    [SerializeField] private Image mapImage;
    [SerializeField] private Image point;
    [SerializeField] private Image pinnedPoint;
    public float multiplicator;


    public override void Close()
    {
        lettersPanel.Close();
        base.Close();
    }

    public override void Open()
    {
        base.Open();
        lettersPanel.Open();
        SetPoints();
    }

    public override void Awake()
    {
        base.Awake();
        lettersPanel.Awake();
    }

    public override void SouthButton()
    {
        base.SouthButton();
        lettersPanel.SouthButton();
        SetPoints();
    }

    public override void BottomDPad()
    {
        base.BottomDPad();
        lettersPanel.BottomDPad();
        SetPoints();
    }

    public override void TopDPad()
    {
        base.TopDPad();
        lettersPanel.TopDPad();
        SetPoints();
    }

    void SetPoints()
    {
        Vector3 pos = (lettersPanel.ReturnPosOfLetter() * multiplicator);
        point.rectTransform.localPosition = new Vector3(pos.x, pos.y, 0);
        if (lettersPanel.pinnedCoordinates != null)
        {
            pinnedPoint.enabled = true;
            Vector3 pinnedPos = (lettersPanel.ReturnPosOfPinnedLetter()*multiplicator);
            pinnedPoint.rectTransform.localPosition = new Vector3(pinnedPos.x, pinnedPos.y, 0);
        }
        else
        {
            pinnedPoint.enabled = false;
        }
    }
}
