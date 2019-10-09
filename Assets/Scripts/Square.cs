﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public Color selectedColor;
    private Color originalColor;

    // Start is called before the first frame update
    void Start()
    {
        originalColor = transform.parent.GetComponent<MeshRenderer>().material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "VojakStred" && other.GetComponentInParent<PlacableObject>().isGrabbed)
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