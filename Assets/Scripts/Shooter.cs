using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK; 

[RequireComponent(typeof(TriggerGrabAndUse))]
public class Shooter : MonoBehaviour
{
    [SerializeField]
    private GameObject shootPoint;

    [SerializeField]
    private int bulletCount = 1;

    [SerializeField]
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material disabledColor;

    [SerializeField]
    private ParticleSystem shotParticles;

    [SerializeField]
    private Animator shotTrajectileAnimator;

    [SerializeField]
    private int damage;

    [SerializeField]
    private AudioSource shootSound;

    [SerializeField]
    private MeshRenderer meshRendererRocket;

    private PlayableObject playableObject;

    private bool shootingEnabled = true;
    private Material[] materialsOriginal;
    private Material[] materialsDisabled;
    private Material materialOriginal;

    private int originalBulletCount;

    private int layerMask = 1 << 8;

    private TriggerGrabAndUse grabMechansm;

    /**
       * A normal member called on Start - includes preparation of materials for enabled/disabled weapon (based on whether weapon is within restricted area or not).
       */
    void Start()
    {
        shotParticles.Stop();
        GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += (object sender, InteractableObjectEventArgs e) => { Shoot(); };

        GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += ObjectUngrabbed;
        originalBulletCount = bulletCount;

        materialsOriginal = new Material[meshRenderer.materials.Length];
        materialsDisabled = new Material[meshRenderer.materials.Length];
        materialOriginal = meshRendererRocket == null ? null : meshRendererRocket.material;
        
        for (var i = 0; i < meshRenderer.materials.Length; i++)
        {
            materialsDisabled[i] = disabledColor;
            materialsOriginal[i] = meshRenderer.materials[i];
        }

        grabMechansm = GetComponent<TriggerGrabAndUse>();

        playableObject = GetComponentInParent<PlayableObject>();
        
    }

    /**
       * A normal member to take action of Shooting with Soldier's weapon.
       * (e.g. Soldier was wrongly placed)
       */
    public void Shoot()
    {
        if (bulletCount > 0 && shootingEnabled)
        {
            shotParticles.Play();
            if (shootSound != null)
            {
                shootSound.Play(0);
            }
            
            shotTrajectileAnimator.Play(shotTrajectileAnimator.name);

            RaycastHit hit;

            if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit, 100, layerMask))
            {
                HealthControl healthControl = hit.transform.GetComponent<HealthControl>();
                if (healthControl != null)
                {
                    healthControl.TakeDamage(damage);
                }
            }
            bulletCount--;
        }
        else
        {
            //TODO: upozornenie ze uz nema naboje a mal by pustit zbran
        }

        if(bulletCount == 0)
        {
            grabMechansm.ActionFinished();
        }
    }

    public bool TeamHit()
    {
        RaycastHit hit;
        if (Physics.Raycast(shootPoint.transform.position, shootPoint.transform.forward, out hit, 100, layerMask))
        {
            if (hit.transform.tag == "EnemySoldier")    //team hit
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;   //im not sure
    }

    /**
       * A normal member used for ungrabbing a Weapon in VR
       * (e.g. Soldier was wrongly placed)
       */
    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        if (bulletCount == 0 || GameManager.Instance.gamemode == GameManager.GameMode.MENU)
        {
            playableObject.AfterFinishedAction();
            ControlShooting(true);
        }
    }

    /**
       * A normal member to reset bullets of a weapon.
       * (e.g. Soldier was wrongly placed)
       */
    public void ResetWeapon() {
        bulletCount = originalBulletCount;
    }

    /**
       * A normal member to enable/disable weapon based on whether it is positioned within a restricted area.
       */
    public void ControlShooting(bool value)
    {
        // To avoid a sequence of coincident changes
        if (value != shootingEnabled)
        {
            shootingEnabled = value;
            meshRenderer.materials = value ? materialsOriginal : materialsDisabled;
            if (materialOriginal != null)
            {
                meshRendererRocket.material = value ? materialOriginal : disabledColor;
            }
        }
    }
    /**
       * A getter for weapon's damage.
       */
    public int GetWeaponDamage()
    {
        return damage;
    }
}
