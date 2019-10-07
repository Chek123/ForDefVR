using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlacableObject : MonoBehaviour
{
    public const float SizeOfSquare = 0.5f;

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
        if (collision.gameObject.tag == "Ihrisko")
        {
            if (!isScaled && !isGrabbed)
            {
                // ToDo: Pri polozeni na podlozku a pusteni, grabnuty objekt sa nezvacsi
                this.transform.localScale *= 5;
                //                this.transform.position = collision.transform.position + new Vector3(0f, collision.transform.localScale.y / 2, 0f)
                //                + new Vector3(0f, transform.localScale.y / 2, 0f);

                this.transform.position = new Vector3( (((int)(this.transform.position.x / SizeOfSquare))*SizeOfSquare), this.transform.localScale.y / 2, (((int)(this.transform.position.z / SizeOfSquare)) * SizeOfSquare));
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
