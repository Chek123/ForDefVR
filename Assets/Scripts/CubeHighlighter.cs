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

    /**
       * A normal member to detect if Soldier is hovering over the "Policko"
       * "Policko" highlighting depends on whether it is already occupied or not
       * @param other - Collider.
       */
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

    /**
       * A normal member to detect Soldier has left the area of "Policko"
       * @param other - Collider.
       */
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

    /**
       * A normal member to reset color of the "Policko" from highlighted to normal
       * @param other - Collider.
       */
    public void ResetColor()
    {
        transform.parent.GetComponent<MeshRenderer>().material.color = originalColor;
    }

    /**
       * A normal member to set the color of the "Policko" to highlighted
       * @param other - Collider.
       */
    public void HighLight()
    {
        transform.parent.GetComponent<MeshRenderer>().material.color = selectedColor;

    }
}
