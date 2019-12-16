using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject soldierModel;

    [SerializeField]
    private GameObject weapon;

    public WeaponController weaponController;

    private Vector3 originalWeaponPosition;
    private Quaternion originalWeaponRotation;

    private Transform sceneObjects;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectChoosenToPlay;

        sceneObjects = GameObject.FindGameObjectWithTag("SceneObjects").transform;
        originalWeaponPosition = weapon.transform.position;
        originalWeaponRotation = weapon.transform.rotation;
    }

    private void ObjectChoosenToPlay(object sender, InteractableObjectEventArgs e)
    {
        if (GameManager.Instance.gamemode == GameManager.GameMode.PLAYER_TURN)
        {
            Debug.Log("Player turn");
            GameManager.Instance.SetRolePlayMode();
            sceneObjects.localScale *= 3 * PlayAreaRealSize.GetFactor();
            VRTK_DeviceFinder.PlayAreaTransform().position = transform.position; //teleport to soldier place 
            soldierModel.SetActive(false);
            weapon.GetComponent<VRTK_InteractableObject>().isGrabbable = true;

            weaponController.setPosition(transform.position.x, transform.position.z);
            weaponController.collider.enabled = true; // collider control

        }
    }

    public void AfterFinishedAction()
    {
        GameManager.Instance.SetEnemyTurnMode();
        sceneObjects.localScale /= 3 / PlayAreaRealSize.GetFactor();
        VRTK_DeviceFinder.PlayAreaTransform().position = Vector3.zero;
        soldierModel.SetActive(true);
        weapon.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        weapon.transform.position = originalWeaponPosition;
        weapon.transform.rotation = originalWeaponRotation;

        weapon.GetComponent<Shooter>().ResetWeapon();
        weaponController.collider.enabled = false; // disabling collider to avoid disabling a weapon when collider is moving around the playground

        GetComponent<Enemy>().EnemyController();
    }
}

