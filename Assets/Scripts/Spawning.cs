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

    private Stack<BoxCollider> boxColliders = new Stack<BoxCollider>();
    private HashSet<BoxCollider> boxCollidersSet = new HashSet<BoxCollider>(); 


    // Start is called before the first frame update
    void Start()
    {
        vojakPosition = vojak.transform.position;
        vojakRotation = vojak.transform.rotation;
        vojakName = vojak.name;

        pocetnostCounter.text = "" + vojakSpawnPocetnost;
        Debug.Log(vojak.transform.GetChild(3));

        boxCollider1 = vojak.transform.GetChild(0).GetComponent<BoxCollider>();
        boxCollider2 = vojak.transform.GetChild(2).GetComponent<BoxCollider>();
        boxCollider3 = vojak.transform.GetChild(4).GetComponent<BoxCollider>();
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
                boxCollider1 = vojakSpawned.transform.GetChild(0).GetComponent<BoxCollider>();
                boxCollider2 = vojakSpawned.transform.GetChild(2).GetComponent<BoxCollider>();
                boxCollider3 = vojakSpawned.transform.GetChild(4).GetComponent<BoxCollider>();
                spawned = true;
            }
            other.gameObject.transform.parent.transform.parent = this.transform.parent.transform.parent;
            decreaseCounter();
        }
        if (boxColliders.Count == 3)
        {
            controllColliders(other.gameObject, true);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        PlacableObject placableObject = null;
        if ((placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.wasChanged && !placableObject.isGrabbed && !placableObject.isScaled && spawned)
        {
            Destroy(vojakSpawned);
            increaseCounter();
            other.gameObject.transform.parent.transform.position = vojakPosition;
            other.gameObject.transform.parent.transform.rotation = vojakRotation;
            boxCollider1 = other.gameObject.transform.GetChild(0).GetComponent<BoxCollider>();
            boxCollider2 = other.gameObject.transform.GetChild(2).GetComponent<BoxCollider>();
            boxCollider3 = other.gameObject.transform.GetChild(4).GetComponent<BoxCollider>();
            placableObject.wasChanged = false;
            spawned = false;
        }

        if (boxColliders.Count != 3 && (placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.isGrabbed)
        {
            Debug.Log(other.gameObject);
            controllColliders(other.gameObject, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        PlacableObject placableObject = null;
        if (boxColliders.Count != 3 && (placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.isGrabbed)
        {
            controllColliders(other.gameObject, false);
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
