using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private Shooter shooterScript;
    private GameObject weapon;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        Debug.Log(other.gameObject.transform.parent.name);
        if (other.gameObject.transform.parent.name == "Weapon")
        {
            other.gameObject.transform.parent.GetComponent<Shooter>().ControlShooting(true);
        }*/
        ControlShooting(other.gameObject.transform.parent, true);
    }

    private void OnTriggerExit(Collider other)
    {
        /*
        Debug.Log(other.gameObject.transform.parent.name);
        if (other.gameObject.transform.parent.name == "Weapon")
        {
            other.gameObject.transform.parent.GetComponent<Shooter>().ControlShooting(false);
        }
        */
        ControlShooting(other.gameObject.transform.parent, false);
    }

    private void ControlShooting(Transform parent, bool flag)
    {
        if (parent.name == "Weapon")
        {
            if (weapon == null || !parent.Equals(weapon))
            {
                weapon = parent.gameObject;
                shooterScript = parent.GetComponent<Shooter>();
            }
            shooterScript.ControlShooting(flag);
        }
    }

    public void setPosition(float x, float z)
    {
        this.transform.position = new Vector3(x, this.transform.position.y, z);
    }
}
