using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeHighlighter : MonoBehaviour
{
    public Color selectedColor;
    private Color originalColor;
    public GameObject occupyingObject;
    private PlacableObject placableObject;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
        occupyingObject = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VojakStred" && other.GetComponentInParent<PlacableObject>().isGrabbed && occupyingObject == null)
        {
            HighLight();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "VojakStred")
        {
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
