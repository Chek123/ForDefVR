using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using System.Threading;

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
    private GameObject[] enemySoldiers;
    private GameObject[] playerSoldiers;


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
        if (GameManager.Instance.gamemode == GameManager.GameMode.ENEMY_CHOOSING)
        {
            GameManager.Instance.SetRolePlayMode();
            sceneObjects.localScale *= 3 * PlayAreaRealSize.GetFactor();
            VRTK_DeviceFinder.PlayAreaTransform().position = transform.position; //teleport to soldier place 
            soldierModel.SetActive(false);
            weapon.GetComponent<VRTK_InteractableObject>().isGrabbable = true;

            weaponController.setPosition(transform.position.x, transform.position.z);
            weaponController.collider.enabled = true; // collider control

        }
    }

    private void DoEnemyTurn()
    {
        Debug.Log("Its enemy turn :)");

        //Random choose enemy soldier and target
        //TODO: More intelligent solution

        enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        int soldier_id = Random.Range(0, enemySoldiers.Length);
        var enemy = enemySoldiers[soldier_id];

        playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");
        soldier_id = Random.Range(0, playerSoldiers.Length);
        var player = playerSoldiers[soldier_id];


        // Find weapon of chosen enemy soldier


        var weapon = enemy.transform.Find("Weapon");
        if (weapon)
        {
            weapon.LookAt(player.transform);
            weapon.localPosition += new Vector3(-0.15f, 0.3f, 0);  // Set weapon little bit higher - TODO: animation. 
            var shooter = weapon.GetComponent<Shooter>();
            shooter.EnemyShoot();       // TODO: raycasting inside EnemyShoot method
        }

        GameManager.Instance.SetEnemyChoosingMode();
    }

    public void AfterFinishedAction()
    {
        //GameManager.Instance.SetEnemyChoosingMode();
        GameManager.Instance.SetEnemyTurnMode();
        sceneObjects.localScale /= 3 / PlayAreaRealSize.GetFactor();
        VRTK_DeviceFinder.PlayAreaTransform().position = Vector3.zero;
        soldierModel.SetActive(true);
        weapon.GetComponent<VRTK_InteractableObject>().isGrabbable = false;
        weapon.transform.position = originalWeaponPosition;
        weapon.transform.rotation = originalWeaponRotation;

        weapon.GetComponent<Shooter>().ResetWeapon();
        weaponController.collider.enabled = false; // disabling collider to avoid disabling a weapon when collider is moving around the playground

        Invoke("DoEnemyTurn", 5.0f);   //invokes DoEnemyTurn() method with 5 sec delay
    }
}

