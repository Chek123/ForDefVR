using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlacableObject : MonoBehaviour
{
    public const float SizeOfSquare = 0.5f;

    public bool isScaled = false;
    private bool isGrabbed = false;

    private bool onCollision = false;
    private Transform lastCollisionObj;

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
        }
        isGrabbed = true;
    }

    private void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;
        if (onCollision)
        {
            SnapToObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Policko")
        {
            lastCollisionObj = collision.transform;
            if ( !isGrabbed)
            {
                SnapToObject();
            }
            onCollision = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        onCollision = false;
    }

    private void SnapToObject()
    {
        if (!isScaled)
        {
            this.transform.localScale *= 5;
            this.transform.position = lastCollisionObj.position + new Vector3(0f, lastCollisionObj.localScale.y / 2, 0f)
                + new Vector3(0f, transform.localScale.y / 2, 0f);
            this.transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            lastCollisionObj.GetComponentInChildren<Square>().ResetColor();
            isScaled = true;
        }

    }
}
