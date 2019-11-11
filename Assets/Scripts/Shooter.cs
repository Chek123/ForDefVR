using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK; 

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject shootPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int bulletCount = 3;

    [SerializeField]
    private PlayableObject playableObject;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += Shoot;
        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;

    }

    private void Shoot(object sender, InteractableObjectEventArgs e)
    {
        if (bulletCount > 0)
        {
            var bullet = GameObject.Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
            bullet.GetComponent<Rigidbody>().velocity += transform.forward * bulletSpeed;
            bulletCount--;
        }
        else
        {
            //TODO: upozornenie ze uz nema naboje a mal by pustit zbran
        }
    }

    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (bulletCount == 0)
        {
            playableObject.AfterFinishedAction();
        }
    }
}
