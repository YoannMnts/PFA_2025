using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    [Range(0, 360)] public float speed;
    public bool reversed;
    public GameObject[] paddles;
    public float[] paddlesTimers;

    void Start()
    {
        paddlesTimers = new float[paddles.Length];
        for (int i = 0; i < paddles.Length; i++)
        {
            paddlesTimers[i] = ((360/speed)*(i/6f));
        }
    }
    void Update()
    {
        if (reversed)
        {
            gameObject.transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        }
        else
        {
            gameObject.transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }
        for (int i = 0; i < paddles.Length; i++)
        {
            paddlesTimers[i] += Time.deltaTime;
            if (paddlesTimers[i] >= (360/speed) - 2f)
            {
                if (paddles[i].GetComponent<BoxCollider2D>().enabled == true)
                {
                    paddles[i].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            if (paddlesTimers[i] >= (360/speed) + 1.5f)
            {
                paddles[i].GetComponent<BoxCollider2D>().enabled = true;
                paddlesTimers[i] = 1.5f;
            }
            
        }
    }
    
}
