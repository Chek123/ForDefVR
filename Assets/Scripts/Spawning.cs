using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawning : MonoBehaviour
{
    public GameObject vojak;
    public int vojakSpawnPocetnost;

    public TextMesh pocetnostCounter;

    public GameObject vojakDestroy;
        
    private GameObject vojakSpawned;
    private Vector3 vojakPosition;
    private Quaternion vojakRotation;
        
    private bool spawned = false;
    private bool wasEverMoved = false;

    private String vojakName;

    public GameObject vojakPreFab;

    private BoxCollider boxCollider1;
    private BoxCollider boxCollider2;
    private BoxCollider boxCollider3;

    private List<BoxCollider> enteredVojakBoxColliders;
    private List<BoxCollider> currentVojakBoxColliders;

    private HashSet<String> colliders;

    const int expectedHashSetSize = 3;
   
    // Start is called before the first frame update
    void Start()
    {
        vojakPosition = vojak.transform.position;
        vojakRotation = vojak.transform.rotation;
        vojakName = vojak.name;

        pocetnostCounter.text = "" + vojakSpawnPocetnost;
        colliders = new HashSet<String>();

        currentVojakBoxColliders = vojak.GetComponent<PlacableObject>().getColliders();
        enteredVojakBoxColliders = null;

    }

    private void OnTriggerExit(Collider other)
    {

        if (vojakSpawnPocetnost > 0 && ((vojakSpawned != null && vojakSpawned.GetComponent<PlacableObject>().isGrabbed) || !spawned) && other.gameObject.transform.parent.name == vojakName)
        {
            if (vojakSpawnPocetnost > 1)
            {
                vojakSpawned = Instantiate(vojakPreFab, vojakPosition, vojakRotation) as GameObject;
                vojakSpawned.name = vojakName;
                vojakSpawned.transform.parent = this.transform.parent;
                vojakSpawned.GetComponent<PlacableObject>().spawner = this.gameObject;
                enteredVojakBoxColliders = currentVojakBoxColliders;
                currentVojakBoxColliders = vojakSpawned.GetComponent<PlacableObject>().getColliders();
                controlColliders(true);

                spawned = true;
            }
            other.gameObject.transform.parent.transform.parent = this.transform.parent.transform.parent;
            decreaseCounter();
        }
        if (enteredVojakBoxColliders != null && other.gameObject.transform.parent.GetComponent<PlacableObject>() != null)
        {
            colliders.Add(other.gameObject.name);
                
            if (colliders.Count == expectedHashSetSize)
            {
                //controlColliders(false);
                enteredVojakBoxColliders = null;
                colliders.Clear();
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        PlacableObject placableObject = null;
        if ((placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.wasChanged && !placableObject.isGrabbed && !placableObject.isScaled && spawned && other.gameObject.transform.parent.name == vojakName)
        {
            Destroy(vojakSpawned);
            increaseCounter();
            other.gameObject.transform.parent.transform.position = vojakPosition;
            other.gameObject.transform.parent.transform.rotation = vojakRotation;
            currentVojakBoxColliders = placableObject.boxColliders;
            enteredVojakBoxColliders = null;
            placableObject.wasChanged = false;
            spawned = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        PlacableObject placableObject = null;
        if ((placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.isGrabbed)
        {
            enteredVojakBoxColliders = placableObject.getColliders();
            controlColliders(true);
        }
    }

    public void OnWrongPlacement()
    {
        if (vojakSpawnPocetnost == 0)
        {
            vojakSpawned = Instantiate(vojakPreFab, vojakPosition, vojakRotation) as GameObject;
            vojakSpawned.name = vojakName;
            spawned = true;
        }
        increaseCounter();

    }

    public void increaseCounter()
    {
        vojakSpawnPocetnost += 1;
        pocetnostCounter.text = "" + vojakSpawnPocetnost;
    }

    public void decreaseCounter()
    {
        vojakSpawnPocetnost -= 1;
        vojakDestroy.GetComponent<VojakDestroy>().destroyed = false;
        pocetnostCounter.text = "" + vojakSpawnPocetnost;
    }
/*
    private void controllColliders(GameObject toIgnore, bool state)
    {
        BoxCollider boxCollider = toIgnore.GetComponent<BoxCollider>();
        if (!state && !boxCollidersSet.Contains(boxCollider))
        {
            boxColliders.Push(boxCollider);
            boxCollidersSet.Add(boxCollider);
            Physics.IgnoreCollision(boxCollider1, boxCollider);
            Physics.IgnoreCollision(boxCollider2, boxCollider);
            Physics.IgnoreCollision(boxCollider3, boxCollider);
            
        }
        else
        {
            boxCollider = boxColliders.Pop();
            while (boxColliders.Count > 0)
            {
                Physics.IgnoreCollision(boxCollider1, boxCollider);
                Physics.IgnoreCollision(boxCollider2, boxCollider);
                Physics.IgnoreCollision(boxCollider3, boxCollider);
                boxCollider = boxColliders.Pop();
            }
            boxCollidersSet.Clear();
        }
    }
*/

    private void controlColliders(bool state) {

        if (state)
        {
            foreach (BoxCollider current in currentVojakBoxColliders)
            {
                foreach (BoxCollider entered in enteredVojakBoxColliders)
                {
                    Physics.IgnoreCollision(current, entered);
                }
            }
        }
        else
        {
            foreach (BoxCollider current in currentVojakBoxColliders)
            {
                foreach (BoxCollider entered in enteredVojakBoxColliders)
                {
                    Physics.IgnoreCollision(current, entered, false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
