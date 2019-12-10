using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private Shooter shooterScript;
    private GameObject weapon;

    public BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        ShootingController(other.gameObject, true);
    }

    private void OnTriggerExit(Collider other)
    {
        ShootingController(other.gameObject, false);
    }
        
    private void ShootingController(GameObject collidingObject, bool flag)
    {
        if (collidingObject.tag == "ShootControl")
        {
            Transform parent = collidingObject.transform.parent;
            if (parent.tag == "Weapon")
            {
                if (weapon == null || !parent.Equals(weapon))
                {
                    weapon = parent.gameObject;
                    shooterScript = parent.GetComponent<Shooter>();
                }
                shooterScript.ControlShooting(flag);
            }
        }
    }

    public void setPosition(float x, float z)
    {
        this.transform.position = new Vector3(x, this.transform.position.y, z);
    }
}
