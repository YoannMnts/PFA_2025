using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StampsPanel : Panel
{
    [SerializeField] private Image[] stampsFrames;
    [SerializeField] public Sprite[] stamps;
    [SerializeField] private TextMeshProUGUI pageText;

    private int count;
    private int currentPage = 0;
    private int stampsPerPage = 4;
    private int pagesCount;

    public override void Awake()
    {
        count = stamps.Length;
        pagesCount = Mathf.CeilToInt(count / stampsPerPage);
        base.Awake();
    }

    void OnEnable()
    {
        DisplayStamps();
        pageText.text = "Page " + (currentPage+1).ToString();
    }
    public override void RightDPad()
    {
        base.LeftDPad();
        currentPage++;
        if (currentPage >= pagesCount)
        {
            currentPage = 0;
        }
        DisplayStamps();
        pageText.text = "Page " + (currentPage+1).ToString();    }

    public override void Close()
    {
        base.Close();
        currentPage = 0;
    }

    public override void LeftDPad()
    {
        base.RightDPad();
        currentPage--;
        if (currentPage < 0)
        {
            currentPage = pagesCount-1;
        }
        DisplayStamps();
        pageText.text = "Page " + (currentPage+1).ToString();
    }

    void DisplayStamps()
    {
        for (int i = 0; i < stampsPerPage; i++)
        {
            int index = (currentPage*stampsPerPage)+i;
            if (index < stamps.Length)
            {
                stampsFrames[i].sprite = stamps[index];
            }
        }
    }
}
