using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(Rigidbody))]
public class PlacableObject : MonoBehaviour
{
    [SerializeField]
    public Spawning spawning;

    private bool isScaled = false;
    private bool wasDestroyed = false;
    private bool enteredSpawningArea = false;

   //private Transform lastCollisionObj;

    
    private Transform snappedOn;
    

    private Rigidbody rigidBody;

    public bool isGrabbed = false;
    public Transform lastCollisionObj;
    public GameObject targetPolicko;
    public bool onCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUnGrabbed;
        rigidBody = GetComponent<Rigidbody>();
    }

    public bool getIsScaled()
    {
        return isScaled;
    }

    public void setIsScaled(bool isScaled)
    {
        this.isScaled = isScaled;
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isScaled)
        {
            this.transform.localScale /= 5;
            isScaled = false;
        }
        isGrabbed = true;
        snappedOn?.GetComponentInChildren<CubeHighlighter>()?.HighLight();
    }

    private void ObjectUnGrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;
        rigidBody.isKinematic = false;
        if (enteredSpawningArea)
        {
            Destroy(this.gameObject);
            spawning.onWrongPlacement(gameObject);
        }
        if (onCollision)
        {
            SnapToObject();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Policko")
        {
            CubeHighlighter cubeHighlighter = collision.gameObject.transform.GetChild(0).GetComponent<CubeHighlighter>();
            if (!isGrabbed)
            {
                /*
                if (cubeHighlighter.occupyingObject == null || cubeHighlighter.occupyingObject == this.gameObject)
                {*/
                    Debug.Log("TargetPolicko " + this.targetPolicko);
                    if (targetPolicko != null)
                    {
                        //lastCollisionObj = collision.gameObject.transform;
                        SnapToObject();
                    }
/*                    if (targetPolicko == collision.gameObject)
                    {
                        cubeHighlighter.occupyingObject = this.gameObject;
                        cubeHighlighter.placableObject = this;
                        SnapToObject();
                    }*/
//                } else 
/*                if (!wasDestroyed && targetPolicko == collision.gameObject)
                {
                    Destroy(this.gameObject); 
                    spawning.onWrongPlacement(gameObject);
                    wasDestroyed = true;
                }
                */
            }
            onCollision = true;
        }
        else if (collision.gameObject.tag == "Ground" && !wasDestroyed)
        {
            spawning.onWrongPlacement(gameObject);
            Destroy(this.gameObject);
            wasDestroyed = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spawning")
        {
            enteredSpawningArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == spawning.gameObject)
        {
            enteredSpawningArea = false;
        }
    }

    private void SnapToObject()
    {
        if (!isScaled)
        {
            rigidBody.isKinematic = true;
            this.transform.localScale *= 5;
            this.transform.position = lastCollisionObj.position;
            this.transform.rotation = Quaternion.identity;
            lastCollisionObj.GetComponentInChildren<CubeHighlighter>().ResetColor();
            isScaled = true;
            snappedOn = lastCollisionObj;
        }
    }
}
