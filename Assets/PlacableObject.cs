using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlacableObject : MonoBehaviour
{

    private bool isScaled = false;
    private bool isGrabbed = false;
    public Transform snap;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUnGrabbed;

    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isScaled)
        {
            this.transform.localScale /= 5;
            isScaled = false;
            isGrabbed = true;
        }
    }

    private void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Policko")
        {
            if (!isScaled && !isGrabbed)
            {
                // ToDo: Pri polozeni na podlozku a pusteni, grabnuty objekt sa nezvacsi
                this.transform.localScale *= 5;
                this.transform.position = collision.transform.position + new Vector3(0f, collision.transform.localScale.y / 2, 0f)
                + new Vector3(0f, transform.localScale.y / 2, 0f);
                this.transform.rotation = Quaternion.identity;
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

                isScaled = true;
            }




        }
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
