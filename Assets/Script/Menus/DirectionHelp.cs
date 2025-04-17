using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.Rendering;
using Vector3 = UnityEngine.Vector3;

public class DirectionHelp : MonoBehaviour 
{
    [SerializeField] private GameObject directionArrow;
    [SerializeField] private GameObject playerPosition;
    public Vector3 target;
    public bool active = false;
    private float distance = 5;
    private float maxX = 10, minX = -10f;
    private float maxY = 6, minY = -6f;
    private GameObject childArrow;


    private void Start()
    {
        childArrow = directionArrow.transform.GetChild(0).gameObject;
    }

    private void LateUpdate()
    {
        if (active)
        {
            Vector3 direction = -(playerPosition.transform.position-target);
            if (direction.magnitude < 20)
            {
                childArrow.GetComponent<SpriteRenderer>().enabled = false;
            }
            else
            {
                childArrow.GetComponent<SpriteRenderer>().enabled = true;
            }
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            directionArrow.transform.Rotate(0,0,-directionArrow.transform.rotation.eulerAngles.z); 
            directionArrow.transform.Rotate(0,0,angle);
            
            distance = 15;
            bool goodPosition = false;
            while (goodPosition == false)
            {
                childArrow.transform.localPosition= new Vector3(distance,0,0);
                Vector3 arrowDistance = childArrow.transform.position - playerPosition.transform.position;
                distance -= 0.05f;
                goodPosition = arrowDistance.x > minX && arrowDistance.x< maxX && arrowDistance.y > minY && arrowDistance.y < maxY;
            }
            
        }
        else
        {
            childArrow.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    
}
