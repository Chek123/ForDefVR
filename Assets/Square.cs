using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{

    public Material Selected;
    public Material notSelected;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Vojak")
        {
            this.GetComponent<MeshRenderer>().material = Selected;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Vojak")
        {
            this.GetComponent<MeshRenderer>().material = notSelected;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
