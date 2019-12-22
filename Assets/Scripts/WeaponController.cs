using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    private Shooter shooterScript;
    private GameObject weapon;

    public BoxCollider collider;

    /**
       * A normal member to detect when a weapon of the selected Soldier is inside the restricted area
       * @param other - Collider.
       */
    private void OnTriggerEnter(Collider other)
    {
        ShootingController(other.gameObject, true);
    }

    /**
       * A normal member to detect when a weapon of the selected Soldier is out of restricted area
       * @param other - Collider.
       */
    private void OnTriggerExit(Collider other)
    {
        ShootingController(other.gameObject, false);
    }

    
    /**
       * Controller for weapon - enabling and disabling the weapon based on its position (within or out of restricted area).
       * @param colliding object - GameObject (weapon expected).
       * @param flag to enable/disable weapon - bool.
       */
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

    /**
       * Sets position of the collider (creates restricted area) to the position of selected player.
       * @param Coordinate x - float.
       * @param Coordinate z - float.
       */
    public void setPosition(float x, float z)
    {
        this.transform.position = new Vector3(x, this.transform.position.y, z);
    }
}
