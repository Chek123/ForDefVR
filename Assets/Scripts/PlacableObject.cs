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

    /**
       * Getter for IsScaled flag - value based on the fact whether the Soldier is scaled (true) or not (false).
       * @return value of flag IsScaled.
       */
    public bool getIsScaled()
    {
        return isScaled;
    }
    
    /**
       * Setter for IsScaled flag - value based on the fact whether the Soldier is scaled (true) or not (false).
       */
    public void setIsScaled(bool isScaled)
    {
        this.isScaled = isScaled;
    }

    /**
       * Getter for isInSpawningArea flag - value based on the fact whether the Soldier is in spawning area (true) or not (false).
       * @return value of flag isInSpawningArea.
       */
    public bool GetIsInSpawningArea()
    {
        return this.isInSpawningArea;
    }

    /**
       * A normal member called on wrong placement of Soldier
       * Used during check of soldiers before game start.
       * (e.g. Soldier was taken from the spawning area and was placed on the playground. If the Soldier is not scaled, he needs to be removed before start).
       */
    public void WrongPlacement()
    {
        spawning.onWrongPlacement(gameObject);
        Destroy(this.gameObject);
    }

    /**
       * A normal member used for grabbing a Soldier in VR
       * @param sender object.
       * @param e InteractableObjectEventArgs.
       */
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (isScaled)
        {
            this.transform.localScale /= 5;
            isScaled = false;
        }
        isGrabbed = true;
        modelAnimator.SetBool("Static", true);

    }
    /**
       * A normal member used for ungrabbing a Soldier in VR
       * @param sender object.
       * @param e InteractableObjectEventArgs.
       */
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

    /**
       * A normal member used for handling Soldier entering collider of "Policko". It can be potentionally snapped to this "Policko" later on.
       * @param collision - Collision.
       * @param e InteractableObjectEventArgs.
       */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Policko")
        {
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

    /**
       * A normal member used for handling Soldier entering collider of Spawning Area.
       * @param other Collider.
       */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Spawning")
        {
            enteredSpawningArea = true;
            isInSpawningArea = true;
        }
    }

    /**
       * A normal member used for handling Soldier exiting collider of Spawning Area.
       * @param other Collider.
       */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == spawning.gameObject)
        {
            enteredSpawningArea = false;
            isInSpawningArea = false;
        }
    }

    /**
       * A normal member used for snapping a Soldier to target "Policko".
       */
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

            modelAnimator.SetBool("Static", false);
        }
    }
}
