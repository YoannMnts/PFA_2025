using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

public class Noria : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed;
    public bool reversed;
    private float distanceTravelled = 0f;
    public GameObject[] paddles; 
    public float[] distancesTravelled;
    

    private float splineLength;

    void Start()
    {
        splineLength = splineContainer.Spline.GetLength();
        distancesTravelled = new float[paddles.Length];
        float spacebetween = splineLength/paddles.Length;
        for (int i = 0; i < paddles.Length; i++)
        {
            distancesTravelled[i] = spacebetween * i;
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < paddles.Length; i++)
        {
            if (distancesTravelled[i] != null)
            {
                if (reversed)
                {
                    distancesTravelled[i] -= speed * Time.deltaTime;
                    if (distancesTravelled[i] <= 0)
                    {
                        distancesTravelled[i] += splineLength;
                    }
                }
                else
                {
                    distancesTravelled[i] += speed * Time.deltaTime;
                    if (distancesTravelled[i] >= splineLength)
                    {
                        distancesTravelled[i] -= splineLength;
                    }
                }
                

                Vector3 positionLocal = splineContainer.Spline.EvaluatePosition(distancesTravelled[i] / splineLength);
                paddles[i].transform.position = splineContainer.transform.TransformPoint(positionLocal);
                
                Vector3 tangentLocal = splineContainer.Spline.EvaluateTangent(distancesTravelled[i]/splineLength);
                Vector3 tangentWorld = splineContainer.transform.TransformDirection(tangentLocal);
                if (tangentWorld != Vector3.zero)
                {
                    float angle = Mathf.Atan2(tangentLocal.y, tangentLocal.x) * Mathf.Rad2Deg;
                    paddles[i].transform.rotation = Quaternion.Euler(0, 0, angle);
                }

                if (distancesTravelled[i] >= 32 && distancesTravelled[i] <= 40)
                {
                    paddles[i].GetComponent<BoxCollider2D>().enabled = false;
                }

                else 
                {
                    if (paddles[i].GetComponent<BoxCollider2D>().enabled == false)
                    {
                        paddles[i].GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
        }
        
    }    
}
