using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayableObject : MonoBehaviour
{

    [SerializeField]
    private Transform sceneObjects;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectUnused;
    }

    private void ObjectUnused(object sender, InteractableObjectEventArgs e)
    {
        sceneObjects.localScale *= 5;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
