using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Spawning : MonoBehaviour
{

    public string vojakTag;
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
        Debug.Log(other.gameObject.tag);
        if (vojakSpawnPocetnost > 0 && ((vojakSpawned != null && vojakSpawned.GetComponent<PlacableObject>().isGrabbed) || !spawned) && other.gameObject.tag == vojakTag)
        {
            vojakSpawned = Instantiate(vojakPreFab, vojakPosition, vojakRotation) as GameObject;
            spawned = true;
            decreaseCounter();
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
