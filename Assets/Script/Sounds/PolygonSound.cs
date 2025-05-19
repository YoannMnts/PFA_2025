using UnityEngine;

public class PolygonSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private PolygonCollider2D riverShape;
    [SerializeField] private Transform listener;
    [SerializeField] private float fadeDistance = 5f; 
    [SerializeField] private OptionsPanel optionsPanel;

    void Update()
    {
        Vector2 listenerPos = listener.position;
        
        float distance = DistanceToPolygon(riverShape, listenerPos);

        if (distance < fadeDistance)
        {
            float t = Mathf.SmoothStep(1f, 0f, distance / fadeDistance)*optionsPanel.volumes[0]*0.1f*optionsPanel.volumes[2]*0.1f;
            audioSource.volume = t;
        }
        else
        {
            audioSource.volume = 0f;
        }
    }
    
    float DistanceToPolygon(PolygonCollider2D poly, Vector2 point)
    {
        float minDistance = float.MaxValue;

        for (int i = 0; i < poly.pathCount; i++)
        {
            Vector2[] path = poly.GetPath(i);

            for (int j = 0; j < path.Length; j++)
            {
                Vector2 a = poly.transform.TransformPoint(path[j]);
                Vector2 b = poly.transform.TransformPoint(path[(j + 1) % path.Length]);
                float d = DistancePointToSegment(point, a, b);
                if (d < minDistance)
                    minDistance = d;
            }
        }

        return minDistance;
    }
    float DistancePointToSegment(Vector2 p, Vector2 a, Vector2 b)
    {
        Vector2 ab = b - a;
        Vector2 ap = p - a;
        float t = Mathf.Clamp01(Vector2.Dot(ap, ab) / ab.sqrMagnitude);
        Vector2 projection = a + t * ab;
        return Vector2.Distance(p, projection);
    }
}
