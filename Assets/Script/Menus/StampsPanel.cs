using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StampsPanel : Panel
{
    [SerializeField] private RectTransform selectionPanel;
    [SerializeField] private GridLayoutGroup stampsFramesContainer;
    private Image[] stampsFrames;
    [SerializeField] private Image zoomedStamp;
    [SerializeField] public Sprite[] stamps;
    [SerializeField] private Sprite notUnlockedStamp;
    private bool[] unlocked;
    private int count;
    private int rowCount;
    private int lastUnlocked;
    private int currentSelected;

    public override void Awake()
    {
        int childrenCount = stampsFramesContainer.transform.childCount;
        count = childrenCount;
        unlocked = new bool[count];
        rowCount = 5;
        stampsFrames = new Image[count];
        for (int i = 0; i < count; i++)
        {
            unlocked[i] = false;
            stampsFrames[i] = stampsFramesContainer.transform.GetChild(i).GetComponent<Image>();
        }
        
        base.Awake();
    }

    public override void Open()
    {
        base.Open();
        currentSelected = 0;
        for (int i = 0; i < count; i++) 
        { 
            if (unlocked[i] == false) 
            { 
                stampsFrames[i].sprite = notUnlockedStamp;
            }
            else 
            { 
                stampsFrames[i].sprite = stamps[i];
            }
        }
        selectionPanel.position = stampsFrames[0].rectTransform.position;
        DisplayStamps();
    }

    public override void RightDPad()
    {
        base.LeftDPad();
        currentSelected++;
        if (currentSelected >= count)
        {
            currentSelected = 0;
        }
        DisplayStamps();
    }


    public override void LeftDPad()
    {
        base.RightDPad();
        currentSelected--;
        if (currentSelected < 0)
        {
            currentSelected = count - 1;
        }
        DisplayStamps();
    }

    public override void TopDPad()
    {
        base.TopDPad();
        currentSelected -= 10;
        if (currentSelected < 0)
        {
            currentSelected += count;
        }
        DisplayStamps();
    }

    public override void BottomDPad()
    {
        base.BottomDPad();
        currentSelected += 10;
        if (currentSelected >= count)
        {
            currentSelected -= 50;
        }
        DisplayStamps();
    }

    void DisplayStamps()
    {
        selectionPanel.position = stampsFrames[currentSelected].rectTransform.position;
        if (unlocked[currentSelected])
        {
            zoomedStamp.sprite = stamps[currentSelected];
        }
        else
        {
            zoomedStamp.sprite = notUnlockedStamp;
        }
        
    }
}
