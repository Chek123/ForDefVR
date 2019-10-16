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

    public GameObject vojakPreFab;


    // Start is called before the first frame update
    void Start()
    {
        vojakPosition = vojak.transform.position;
        vojakRotation = vojak.transform.rotation;

        pocetnostCounter.text = "" + vojakSpawnPocetnost;
    }

    private void OnTriggerExit(Collider other)
    {
        if (vojakSpawnPocetnost > 0 && ((vojakSpawned != null && vojakSpawned.GetComponent<PlacableObject>().isGrabbed) || !spawned) && other.gameObject.transform.parent.name == vojak.name)
        {
            vojakSpawned = Instantiate(vojakPreFab, vojakPosition, vojakRotation) as GameObject;
            vojakSpawned.name = vojak.name;
            spawned = true;
            decreaseCounter();
        }      
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        PlacableObject placableObject = null;
        if ((placableObject = other.gameObject.transform.parent.GetComponent<PlacableObject>()) != null && placableObject.wasChanged && !placableObject.isGrabbed && !placableObject.isScaled && spawned)
        {
            Debug.Log(other.gameObject.name);
            Destroy(vojakSpawned);
            increaseCounter();
            other.gameObject.transform.parent.transform.position = vojakPosition;
            other.gameObject.transform.parent.transform.rotation = vojakRotation;
            placableObject.wasChanged = false;
            spawned = false;
        }
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


    // Update is called once per frame
    void Update()
    {
        
    }
}
