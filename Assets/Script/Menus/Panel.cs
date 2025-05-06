using UnityEngine;

public class Panel : MonoBehaviour
{
    public GameObject panel;
    public DeliveryManager deliveryManager;

    public virtual void Awake()
    {
 
    }
    public virtual void Open()
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

    public virtual void WestButton()
    {
        
    }
}
