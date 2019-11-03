using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacableLocation : MonoBehaviour
{

    public GameObject occupyingObject;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
//        Debug.Log("TU");
        if (occupyingObject == null)
        {
            occupyingObject = collision.gameObject;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }
}
