using System.Collections;
using TMPro;
using UnityEngine;

public class Notification : SoundObject
{
    [SerializeField]
    private RectTransform letterPopUp, rewardPopUp;
    [SerializeField]
    private TextMeshProUGUI acornsText, stampText;
    [SerializeField]
    private TextMeshProUGUI letterText;

    private float timeOfShowUp = 4f;
    private float speed = 700;

    public override void Start()
    {
        base.Start();
        letterPopUp.anchoredPosition = new Vector3(-1300, 400,0);
        rewardPopUp.anchoredPosition = new Vector3(-1300, 200,0);
    }


    public IEnumerator ShowUpLetter(string receiver)
    {
        PlaySound(clips[0],SoundType.Effects);
        letterPopUp.anchoredPosition = new Vector3(-1300, 400,0);
        letterText.text = "Nouvelle lettre pour <color=#D70000><b>" + receiver + " </b></color>!";
        while (letterPopUp.anchoredPosition.x < -760)
        {
            Vector3 pos = letterPopUp.anchoredPosition;
            pos.x += speed*Time.deltaTime;
            letterPopUp.anchoredPosition = pos;
            yield return null;
        }
        yield return new WaitForSeconds(timeOfShowUp);
        StartCoroutine(GetOutLetter());
    }

    public IEnumerator GetOutLetter()
    {
        while (letterPopUp.anchoredPosition.x > -1300)
        {
            Vector3 pos = letterPopUp.anchoredPosition;
            pos.x -= speed*2*Time.deltaTime;
            letterPopUp.anchoredPosition = pos;
            yield return null;
        }
    }
    public IEnumerator ShowUpReward(int acornsCount, bool stampAcquired = false)
    {
        rewardPopUp.anchoredPosition = new Vector3(-1300, 200,0);
        if (acornsCount > 1)
        {
            acornsText.text = "<b>"+acornsCount.ToString() + " glands gagnés !";
        }
        else
        {
            acornsText.text = "<b>"+acornsCount.ToString() + " gland gagné !";
        }
        
        if (stampAcquired)
        {
            stampText.gameObject.SetActive(true);
        }
        else
        {
            stampText.gameObject.SetActive(false);
        }
        while (rewardPopUp.anchoredPosition.x < -760)
        {
            Vector3 pos = rewardPopUp.anchoredPosition;
            pos.x += speed*Time.deltaTime;
            rewardPopUp.anchoredPosition = pos;
            yield return null;
        }
        yield return new WaitForSeconds(timeOfShowUp);
        StartCoroutine(GetOutReward());
    }

    public IEnumerator GetOutReward()
    {
        while (rewardPopUp.anchoredPosition.x > -1300)
        {
            Vector3 pos = rewardPopUp.anchoredPosition;
            pos.x -= speed*2*Time.deltaTime;
            rewardPopUp.anchoredPosition = pos;
            yield return null;
        }
    }

    public void Disappear()
    {
        StopAllCoroutines();
        letterPopUp.anchoredPosition = new Vector3(-1300, 400,0);
        rewardPopUp.anchoredPosition = new Vector3(-1300, 200,0);
    }
}
