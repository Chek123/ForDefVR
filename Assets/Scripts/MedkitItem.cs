using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/**
 * skript, ktory vylieci vojaka, na ktoreho dopadne 
 */ 
[RequireComponent(typeof(ThrowableItem))]
[RequireComponent(typeof(VRTK_InteractableObject))]
public class MedkitItem : MonoBehaviour
{
    [SerializeField]
    [Tooltip("pocet policok, ktore sa doplnia vojakovi")]
    private int healPower = 2;

    private PlayableObject po;
    private bool isUsed = false;
    private GameObject healObject;

    // init
    void Start()
    {
        po = GetComponentInParent<PlayableObject>();
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += (object sender, InteractableObjectEventArgs e) => { Invoke("FinishAction", 2f); };
    }

    /**
     * Klucovy event, ktory sa po naraze do ineho objektu nastavi premmenu healObject
     * a zastavi fyziku na objekte
     */
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Medkit collision with: "+ collision.gameObject.name);

        if(!isUsed && collision.gameObject.tag == "Vojak")
        {
            GetComponent<Rigidbody>().isKinematic = true;
            healObject = collision.gameObject;
            isUsed = true;
        }
    }

    /**
     * metoda, ktora zavola vyliecenie daneho vojaka (zavola sa s 2 sekundovym oneskorenim
     * aby hrac videl, ktoreho vojaka vyliecil)
     */ 
    private void FinishAction()
    {
        healObject?.GetComponent<HealthControl>()?.TakeDamage(-healPower);
        healObject = null;
        isUsed = false;
        po.AfterFinishedAction();
    }
}
