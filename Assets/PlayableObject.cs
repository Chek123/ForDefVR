using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayableObject : MonoBehaviour
{

    [SerializeField]
    private Transform sceneObjects;

    [SerializeField]
    private GameObject soldierModel;

    [SerializeField]
    private GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectUnused;
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        sceneObjects.localScale *= 3;
        soldierModel.SetActive(false);
        VRTK_DeviceFinder.PlayAreaTransform().position = transform.position; //teleport to soldier place
        weapon.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
        //weapon.GetComponent<Rigidbody>().isKinematic = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
