using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [Range(0.1f,20f)]
    public float loadDistance;
    [Range(0.01f,1f)]
    public float speed;
    [Range(0.01f,1f)]
    public float backSpeed;
    private Vector3[] basePositions;
    private GameObject[] parallaxObjects;

    void Start()
    {
        basePositions = new Vector3[transform.childCount];
        parallaxObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            basePositions[i] = transform.GetChild(i).position;
            parallaxObjects[i] = transform.GetChild(i).gameObject;
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < parallaxObjects.Length; i++)
        {
            float distance = Vector2.Distance(new Vector2(basePositions[i].x, basePositions[i].y),
                new Vector2(player.transform.position.x, player.transform.position.y));
            if ( distance < basePositions[i].z*loadDistance && distance > 1f)
                
            {
                if (basePositions[i].z < 50f)
                {
                     parallaxObjects[i].transform.position = basePositions[i] + new Vector3((player.transform.position.x - basePositions[i].x)*(basePositions[i].z*speed)*(-1f),(player.transform.position.y - basePositions[i].y)*(basePositions[i].z*speed)*(-1f),basePositions[i].z);
                }
                else
                {
                    parallaxObjects[i].transform.position = player.transform.position + new Vector3((player.transform.position.x - basePositions[i].x)*backSpeed*(-1f),(player.transform.position.y - basePositions[i].y)*backSpeed*(-1f),basePositions[i].z);
                }
            }
            else
            {
                parallaxObjects[i].transform.position=basePositions[i];
            }
        }
    }
}
