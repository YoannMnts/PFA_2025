using System.Collections;
using Script.DeliverySys;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : Panel
{
    [SerializeField] private LettersPanel lettersPanel;
    [SerializeField] private DeliveryManager deliveryManager;
    [SerializeField] private Image mapImage;
    [SerializeField] private Image point;
    [SerializeField] private Image pinnedPoint;
    [SerializeField] private Image searchPoint;
    [SerializeField] private Image hazelPosition;
    [SerializeField] private Vector3 refPos;
    [SerializeField] private LetterData saveLetterData;
    


    public override void Close()
    {
        lettersPanel.Close();
        base.Close();
    }

    public override void Open()
    {
        base.Open();
        searchPoint.enabled = false;
        lettersPanel.Open();
        lettersPanel.withMap = true;
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
        if (lettersPanel.letters.Length > 0)
        {
            point.enabled = true;
            Vector3 pos = lettersPanel.ReturnPosOfLetter();
            point.rectTransform.localPosition = new Vector3(pos.x, pos.y, 0);
            if (lettersPanel.pinnedCoordinates != null)
            {
                pinnedPoint.enabled = true;
                Vector3 pinnedPos = lettersPanel.ReturnPosOfPinnedLetter();
                pinnedPoint.rectTransform.localPosition = new Vector3(pinnedPos.x, pinnedPos.y, 0);
            }
            else
            { 
                pinnedPoint.enabled = false; 
            }
        }
        else
        {
            point.enabled = false;
            pinnedPoint.enabled = false;
        }

        Vector3 playerPos = deliveryManager.player.transform.position-refPos;
        float multi = 3.55f;
        hazelPosition.rectTransform.localPosition = new Vector3((playerPos.x)*multi, (playerPos.y)*multi, 0);
    }
    

    public override void WestButton()
    {
        base.WestButton();
        StartCoroutine(FindSender());
    }

    IEnumerator FindSender()
    {
        if (searchPoint.enabled == false)
        {
             Vector3? posOnMap = null;
             for (int i = 0; i < deliveryManager.LetterDataTab.Length; i++)
             {
                 if (deliveryManager.LetterDataTab[i] == saveLetterData)
                 {
                     continue;
                 }
                 bool isAvailable = true;
                 for (int j = 0; j < deliveryManager.LetterDataTab[i].dependencies.Length; j++)
                 { 
                     if (!deliveryManager.completedLetters.Contains(deliveryManager.LetterDataTab[i].dependencies[j]))
                     { 
                         isAvailable = false;
                     }
                 }
                 if (isAvailable)
                 {
                     if (!deliveryManager.completedLetters.Contains(deliveryManager.LetterDataTab[i]))
                     {
                         for (int j = 0; j < deliveryManager.ActiveLetter.Count; j++)
                         {
                             if (deliveryManager.ActiveLetter[j].letterData == deliveryManager.LetterDataTab[i])
                             { 
                                 isAvailable = false;
                             }
                         }
                     }
                     else
                     {
                         isAvailable = false;
                     }
                 }
                 if (isAvailable)
                 {
                     posOnMap = deliveryManager.LetterDataTab[i].sender.mapPosition;
                     break;
                 }
             }
             if (posOnMap != null)
             {
                 searchPoint.enabled = true;
                 searchPoint.rectTransform.localPosition = new Vector3(posOnMap.Value.x, posOnMap.Value.y, 0);
                 searchPoint.color = new Color(1, 1, 1, 0);
                 while (searchPoint.color.a<1)
                 {
                     Color color = searchPoint.color;
                     Color newColor = new Color(color.r, color.g, color.b, color.a+(1.5f*Time.deltaTime));
                     searchPoint.color = newColor;
                     yield return null;
                 }
                 yield return new WaitForSeconds(0.1f);
                 while (searchPoint.color.a>0)
                 {
                     Color color = searchPoint.color;
                     Color newColor = new Color(color.r, color.g, color.b, color.a-(0.8f*Time.deltaTime));
                     searchPoint.color = newColor;
                     yield return null;
                 }
                 searchPoint.enabled = false;
             }
        }
       
    }
}
