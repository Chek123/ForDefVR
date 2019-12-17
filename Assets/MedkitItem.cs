using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

[RequireComponent(typeof(ThrowableItem))]
[RequireComponent(typeof(VRTK_InteractableObject))]
public class MedkitItem : MonoBehaviour
{
    [SerializeField]
    private int healPower = 2;

    private PlayableObject po;
    private bool isUsed = false;
    private GameObject healObject;

    // Start is called before the first frame update
    void Start()
    {
        po = GetComponentInParent<PlayableObject>();
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += (object sender, InteractableObjectEventArgs e) => { Invoke("FinishAction", 2f); };
    }

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

    private void FinishAction()
    {
        healObject?.GetComponent<HealthControl>()?.TakeDamage(-healPower);
        healObject = null;
        isUsed = false;
        po.AfterFinishedAction();
    }
}
