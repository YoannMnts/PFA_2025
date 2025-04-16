using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameObject panel;

    public virtual void Awake()
    {
        panel.SetActive(false);
    }
    public void Open()
    {
        panel.SetActive(true);
    }

    public virtual void Close()
    {
        panel.SetActive(false);
    }

    public virtual void LeftDPad()
    {
        
    }
    public virtual void RightDPad()
    {
        
    }
    public virtual void TopDPad()
    {
        
    }
    public virtual void BottomDPad()
    {
        
    }
    public virtual void SouthButton()
    {

    }
    public virtual void EastButton()
    {
        
    }
    
}
