using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class PlayableObject : MonoBehaviour
{
    [SerializeField]
    private GameObject soldierModel;

    [SerializeField]
    [Tooltip("Weapon or medkit object")]
    private GameObject UsableItem;

    [SerializeField]
    private GameObject soldierCollider;

    public WeaponController weaponController;

    [SerializeField]
    private bool canBeChosenToPlay = true;

    private Vector3 originalItemPosition;
    private Quaternion originalItemRotation;

    private Transform sceneObjects;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK_InteractableObject>().InteractableObjectUnused += ObjectChoosenToPlay;

        sceneObjects = GameObject.FindGameObjectWithTag("SceneObjects").transform;
        if (UsableItem)
        {
            originalItemPosition = UsableItem.transform.position;
            originalItemRotation = UsableItem.transform.rotation;
        }
    }

    /**
       * A normal member allowing to choose a Soldier during the gameplay
       * @param sender object.
       * @param e InteractableObjectEventArgs.
       */
    private void ObjectChoosenToPlay(object sender, InteractableObjectEventArgs e)
    {
        if (GameManager.Instance.gamemode == GameManager.GameMode.PLAYER_TURN && canBeChosenToPlay)
        {
            Debug.Log("Player turn");
            GameManager.Instance.SetRolePlayMode();
            sceneObjects.localScale *= 2.5f / PlayAreaRealSize.GetFactor();
            VRTK_DeviceFinder.PlayAreaTransform().position = transform.position; //teleport to soldier place 
            soldierModel.SetActive(false);
            soldierCollider.SetActive(false);
            UsableItem.GetComponent<VRTK_InteractableObject>().isGrabbable = true;
            weaponController.setPosition(transform.position.x, transform.position.z);
            weaponController.collider.enabled = true; // collider control

        }
    }

    /**
       * A normal member to be called after the user is done with playing in the role of the chosen Soldier.
       */
    public void AfterFinishedAction()
    {
        sceneObjects.localScale /= 2.5f / PlayAreaRealSize.GetFactor();
        VRTK_DeviceFinder.PlayAreaTransform().position = Vector3.zero;
        soldierModel.SetActive(true);
        soldierCollider.SetActive(true);
        UsableItem.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        UsableItem.transform.position = originalItemPosition;
        UsableItem.transform.rotation = originalItemRotation;

        UsableItem.GetComponent<Shooter>()?.ResetWeapon(); 
        weaponController.collider.enabled = false; // disabling collider to avoid disabling a weapon when collider is moving around the playground

        // if gamemode is MENU (= level ended), enemy turn cannot be executed
        if (GameManager.Instance.gamemode != GameManager.GameMode.MENU)
        {
            GetComponent<Enemy>().EnemyController();
        }
    }
}

