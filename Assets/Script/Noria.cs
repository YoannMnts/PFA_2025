using UnityEngine;
using UnityEngine.Splines;

public class Noria : MonoBehaviour
{
    public SplineContainer splineContainer;
    public float speed; 
    private float distanceTravelled = 0f;

    private float splineLength;

    void Start()
    {
        splineLength = splineContainer.Spline.GetLength();
    }

    void Update()
    {
        distanceTravelled += speed * Time.deltaTime;
        if (distanceTravelled >= splineLength)
        {
            distanceTravelled -= splineLength;
        }
        Vector3 positionLocal = splineContainer.Spline.EvaluatePosition(distanceTravelled / splineLength);
        transform.position = splineContainer.transform.TransformPoint(positionLocal);
    }    
}
