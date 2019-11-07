using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlacableObject : MonoBehaviour
{
    public const float SizeOfSquare = 0.5f;

    public GameObject vojakDestroy;
    public GameObject spawner;

    public bool isScaled = false;
    public bool isGrabbed = false;
    public bool wasChanged = false;
    private bool wasDestroyed = false;

    private bool onCollision = false;
    private Transform lastCollisionObj;

    private GameObject targetPolicko;
    private Transform snappedOn;
    

    public Spawning spawning;
    private Rigidbody rigidBody;

    public List<BoxCollider> boxColliders;
    public GameObject cube;
    

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUnGrabbed;
        spawning = spawner.gameObject.GetComponent<Spawning>();
        rigidBody = GetComponent<Rigidbody>();
        vojakDestroy = GameObject.Find("Kos");
    }

    public List<BoxCollider> getColliders()
    {
        return this.boxColliders;
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isScaled)
        {
            this.transform.localScale /= 5;
            isScaled = false;
            vojakDestroy.GetComponent<VojakDestroy>().destroyed = false;
        }
        isGrabbed = true;
        wasChanged = true;

//        transform.localPosition += Vector3.up/100; // [FIX] vojak sa obcas zasekne do podlahy

        snappedOn?.GetComponentInChildren<CubeHighlighter>()?.HighLight();
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
        Debug.Log(collision.gameObject);
        if (collision.gameObject.tag == "Policko")
        {
            CubeHighlighter cubeHighlighter = collision.gameObject.transform.GetChild(0).GetComponent<CubeHighlighter>();
            lastCollisionObj = collision.transform;
            if ( !isGrabbed)
            {
                if (cubeHighlighter.occupyingObject == null || cubeHighlighter.occupyingObject == this.gameObject)
                {
                    if (targetPolicko == null)
                    {
                        targetPolicko = collision.gameObject;
                        Debug.Log("Targetpolicko" + targetPolicko);
                    }
                    if (targetPolicko == collision.gameObject)
                    {
                        cubeHighlighter.occupyingObject = this.gameObject;
                        SnapToObject();
                    }
                }
                else if (!wasDestroyed && targetPolicko == collision.gameObject)
                {
                    Destroy(this.gameObject);
                    spawning.OnWrongPlacement();
                    wasDestroyed = true;
                }
                    
            }
            onCollision = true;
        }
        else if (collision.gameObject.tag == "Ground" && !wasDestroyed)
        {
            Destroy(this.gameObject);
            spawning.OnWrongPlacement();
            wasDestroyed = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        onCollision = false;
        
        if (collision.gameObject.tag == "Policko")
        {
            targetPolicko = null;
            collision.gameObject.transform.GetChild(0).GetComponent<CubeHighlighter>().occupyingObject = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void SnapToObject()
    {
        if (!isScaled)
        {
            this.transform.localScale *= 5;
            this.transform.position = lastCollisionObj.position + new Vector3(0f, lastCollisionObj.localScale.y / 2 - cube.transform.localScale.y , 0f)
                + new Vector3(0f, transform.localScale.y / 2, 0f);
            this.transform.rotation = Quaternion.identity;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            lastCollisionObj.GetComponentInChildren<CubeHighlighter>().ResetColor();
            isScaled = true;
            snappedOn = lastCollisionObj;
        }

    }
}
