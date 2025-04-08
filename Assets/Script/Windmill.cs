using UnityEngine;

public class Windmill : MonoBehaviour
{
    [Range(-360f, 360)] public float speed;

    void Update()
    {
        gameObject.transform.Rotate(Vector3.back, speed * Time.deltaTime);
    }
}
