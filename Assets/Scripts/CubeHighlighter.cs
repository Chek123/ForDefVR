using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHighlighter : MonoBehaviour
{
    public Color selectedColor;
    private Color originalColor;
    private GameObject occupyingObject;
    private PlacableObject placableObject;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        occupyingObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VojakStred" && other.GetComponentInParent<PlacableObject>().isGrabbed)
        {
            HighLight();
        }

        if (occupyingObject == null && other.gameObject.name == "Vojak")
        {
            occupyingObject = other.gameObject;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (occupyingObject != null && other.gameObject != occupyingObject && other.gameObject.name == "Vojak" && !(placableObject = other.GetComponentInParent<PlacableObject>()).isGrabbed)
        {
            placableObject.spawning.OnWrongPlacement();
            Destroy(other.gameObject.transform.parent.gameObject);
            ResetColor();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VojakStred")
        {
            ResetColor();
        }
        if (other.gameObject.tag == "Vojak" && other.gameObject == occupyingObject)
        {
            occupyingObject = null;
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
