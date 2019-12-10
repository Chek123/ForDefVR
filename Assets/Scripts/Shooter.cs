using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK; 

public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject shootPoint;

    [SerializeField]
    private GameObject RaycastPoint;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    [SerializeField]
    private int bulletCount = 3;

    [SerializeField]
    private PlayableObject playableObject;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material disabledColor;

    [SerializeField]
    private GameObject Pointer;

    
    public bool rayCast = false;

    private bool shootingEnabled = true;
    private Material[] materialsOriginal;
    private Material[] materialsDisabled;

    private int originalBulletCount;
    private int layerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += Shoot;

        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;
        originalBulletCount = bulletCount;

        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;

        materialsOriginal = new Material[meshRenderer.materials.Length];
        materialsDisabled = new Material[meshRenderer.materials.Length];
        
        for (var i = 0; i < meshRenderer.materials.Length; i++)
        {
            materialsDisabled[i] = disabledColor;
            materialsOriginal[i] = meshRenderer.materials[i];
        }

    }

    private void Update()
    {
        RaycastHit hit;

        if (rayCast && Physics.Raycast(RaycastPoint.transform.position, RaycastPoint.transform.forward, out hit, 100, layerMask))
        {
            Pointer.transform.position = hit.point;
            Pointer.SetActive(true);
        }
        else
        {
            Pointer.SetActive(false);
        }
       
    }

    private void Shoot(object sender, InteractableObjectEventArgs e)
    {
        if (bulletCount > 0 && shootingEnabled)
        {
            RaycastHit hit;
            if (Physics.Raycast(RaycastPoint.transform.position, RaycastPoint.transform.forward, out hit, 100, layerMask))
            {
                HealthControl healthControl = hit.transform.GetComponent<HealthControl>();
                if (healthControl != null)
                {
                    healthControl.HitSoldier();
                }
            }
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
            ControlShooting(true);
        }
    }

    public void ResetWeapon() {
        bulletCount = originalBulletCount;
    }
    public void ControlShooting(bool value)
    {
        // To avoid a sequence of coincident changes
        if (value != shootingEnabled)
        {
            shootingEnabled = value;
            meshRenderer.materials = value ? materialsOriginal : materialsDisabled;
        }
    }
}
