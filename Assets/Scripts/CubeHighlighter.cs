using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHighlighter : MonoBehaviour
{
    public Color selectedColor;
    private Color originalColor;
    public GameObject occupyingObject;
    public PlacableObject placableObject;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        occupyingObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlacableObject placableObject;
        if (other.gameObject.tag == "VojakStred" && !(placableObject = other.GetComponentInParent<PlacableObject>()).getIsScaled())
        {
            if (occupyingObject == null)
            {
                HighLight();
                placableObject.lastCollisionObj = transform.parent.transform;
                placableObject.targetPolicko = this.transform.parent.gameObject;
                this.occupyingObject = other.transform.parent.gameObject;
            }
            else
            {
                placableObject.lastCollisionObj = placableObject.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VojakStred")
        {
            if (occupyingObject == other.gameObject.transform.parent.gameObject)
            {
                PlacableObject placableObject = other.GetComponentInParent<PlacableObject>();
                placableObject.onCollision = false;
                this.occupyingObject = null;
            }
            ResetColor();
        }
        
    }

    public void ResetColor()
    {
        transform.parent.GetComponent<MeshRenderer>().material.color = originalColor;
    }

    public void HighLight()
    {
        transform.parent.GetComponent<MeshRenderer>().material.color = selectedColor;

    }
}
