using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(RandomFancyAnimationSwitch))]
[RequireComponent(typeof(Rigidbody))]
public class PlacableObject : MonoBehaviour
{
    [SerializeField]
    public Spawning spawning;

    [SerializeField]
    public AudioSource SnapSound;

    private Animator modelAnimator;


    private bool isScaled = false;
    private bool wasDestroyed = false;
    private bool enteredSpawningArea = false;
    private bool isInSpawningArea = true;

   //private Transform lastCollisionObj;

    
    private Transform snappedOn;
    

    private Rigidbody rigidBody;

    [HideInInspector]
    public bool isGrabbed = false;

    [HideInInspector]
    public Transform lastCollisionObj;

    [HideInInspector]
    public GameObject targetPolicko;

    [HideInInspector]
    public bool onCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += ObjectGrabbed;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUnGrabbed;
        rigidBody = GetComponent<Rigidbody>();
        modelAnimator = GetComponent<RandomFancyAnimationSwitch>().soldierAnimator;
    }

    public bool getIsScaled()
    {
        return isScaled;
    }

    public void setIsScaled(bool isScaled)
    {
        this.isScaled = isScaled;
    }

    public bool GetIsInSpawningArea()
    {
        return this.isInSpawningArea;
    }

    public void WrongPlacement()
    {
        spawning.onWrongPlacement(gameObject);
        Destroy(this.gameObject);
    }

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isScaled)
        {
            this.transform.localScale /= 5;
            isScaled = false;
        }
        isGrabbed = true;
        modelAnimator.SetBool("Static", true);

        //snappedOn?.GetComponentInChildren<CubeHighlighter>()?.HighLight();
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

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Policko")
        {
            CubeHighlighter cubeHighlighter = collision.gameObject.transform.GetChild(0).GetComponent<CubeHighlighter>();
            if (!isGrabbed)
            {
                if (targetPolicko != null)
                {
                    SnapToObject();
                }
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
            isInSpawningArea = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == spawning.gameObject)
        {
            enteredSpawningArea = false;
            isInSpawningArea = false;
        }
    }

    private void SnapToObject()
    {
        if (!isScaled && targetPolicko != null && lastCollisionObj.position == targetPolicko.transform.position)
        {
            rigidBody.isKinematic = true;
            this.transform.localScale *= 5;
            this.transform.position = lastCollisionObj.position;
            this.transform.rotation = Quaternion.identity;
            CubeHighlighter cubeHighlighter = lastCollisionObj.GetComponentInChildren<CubeHighlighter>();

            if (cubeHighlighter != null)
            {
                cubeHighlighter.ResetColor();
            }

            SnapSound.Play(0);    
            isScaled = true;
            snappedOn = lastCollisionObj;

            modelAnimator.SetBool("Static", false);
        }
    }
}
