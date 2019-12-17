using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(VRTK_InteractableObject))]
public class ThrowableItem : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasRB = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var ib = GetComponent<VRTK_InteractableObject>();

        ib.InteractableObjectGrabbed += Grabbed;
        ib.InteractableObjectUngrabbed += Ungrabbed;
    }

    private void Grabbed(object sender, InteractableObjectEventArgs e)
    {
        if (!hasRB)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            hasRB = true;
        }
        rb.isKinematic = true;

    }

    private void Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        rb.isKinematic = false;
    }



}
