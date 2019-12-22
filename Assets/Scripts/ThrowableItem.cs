using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/**
 * Umozni hodit predmet po tom ako ho hrac pusti z ruky
 */ 
[RequireComponent(typeof(VRTK_InteractableObject))]
public class ThrowableItem : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasRB = false;

    // Init
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        var ib = GetComponent<VRTK_InteractableObject>();

        ib.InteractableObjectGrabbed += Grabbed;
        ib.InteractableObjectUngrabbed += Ungrabbed;
    }

    /**
     * po grabnuti predmetu sa prida rigidbody ak na objekte nieje a nastavi
     * sa kinematic na true (aby nepadal z ruky)
     */ 
    private void Grabbed(object sender, InteractableObjectEventArgs e)
    {
        if (!hasRB)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            hasRB = true;
        }
        rb.isKinematic = true;

    }

    /**
     * po pusteni predmetu sa nastavi kinematic na false (aby predmet mohol letiet)
     */
    private void Ungrabbed(object sender, InteractableObjectEventArgs e)
    {
        rb.isKinematic = false;
    }



}
