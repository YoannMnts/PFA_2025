using UnityEngine;
using UnityEngine.UI;

public class OptionsPanel : Panel
{
    private int currentLevel = 0 ;
    [SerializeField] private RectTransform selectPad;
    [SerializeField] private Image[] bars;
    public float[] volumes;

    public override void Awake()
    {
        
        volumes = new float[4];
        for (int i = 0; i < volumes.Length; i++)
        {
            volumes[i] = 5;
            bars[i].fillAmount = volumes[i]*0.1f;
        }
        base.Awake();
    }

    void OnEnable()
    {
        selectPad.position = bars[currentLevel].gameObject.GetComponent<RectTransform>().position;
    }
    public override void BottomDPad()
    {
        base.BottomDPad();
        currentLevel += 1;
        if (currentLevel >= 4)
        {
            currentLevel = 0;
        }
        selectPad.position = bars[currentLevel].gameObject.GetComponent<RectTransform>().position ;
       
        
    }

    public override void TopDPad()
    {
        base.TopDPad();
        currentLevel -= 1;
        if (currentLevel < 0)
        {
            currentLevel = 3;
        }
        selectPad.position = bars[currentLevel].gameObject.GetComponent<RectTransform>().position ;
    }

    public override void RightDPad()
    {
        base.RightDPad(); 
        if (volumes[currentLevel] < 10)
        {
            volumes[currentLevel] += 1;
        } 
        bars[currentLevel].fillAmount = (volumes[currentLevel]*0.1f);
        PlaySound(clips[0], SoundType.Effects);
    }
    public override void LeftDPad()
    {
        base.LeftDPad();
        if (volumes[currentLevel] > 0)
        {
            volumes[currentLevel] -= 1;
        }
        bars[currentLevel].fillAmount = (volumes[currentLevel]*0.1f);
        PlaySound(clips[0], SoundType.Effects);
    }
}

