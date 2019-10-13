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

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += Shoot;
    }

    private void Shoot(object sender, InteractableObjectEventArgs e)
    {
        var bullet = GameObject.Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.GetComponent<Rigidbody>().velocity += Vector3.forward * bulletSpeed;
    }
}
