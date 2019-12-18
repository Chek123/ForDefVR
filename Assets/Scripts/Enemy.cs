using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void EnemyController()
    {
        Invoke("DoEnemyTurn", 5.0f);
    }

    private GameObject CountTurnEffectiveness()
    {
        var enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        var playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");

        List<GameObject, GameObject, int> shootInteraction = new List<GameObject, GameObject, int>();
        //EnemySoldier object, PlayerSoldier object, effectiveness integer

        Debug.Log("Max Soldier HP");
        Debug.Log(GameManager.Instance.GetMaxSoldierHP());

        // 1. step
        // Every enemy soldier (with weapon) tries to shoot on every player soldier.
        foreach (var enemy in enemySoldiers)
        {
            var teamHit = 0;
            var weapon = enemy.transform.Find("Weapon");
            if (weapon)
            {
                foreach (var player in playerSoldiers) {
                    var weaponRotation = weapon.rotation;  //store old weapon rotation
                    weapon.LookAt(player.transform);
                    var shooter = weapon.GetComponent<Shooter>();
                    if (shooter.TestShoot())   
                    {
                        teamHit++;
                    }
                    weapon.rotation = weaponRotation;   //set weapon rotation to old state
                }
            }
            Debug.Log("Player on position");
            Debug.Log(enemy.transform.localPosition);
            Debug.Log("Team hits");
            Debug.Log(teamHit);
            if (teamHit == playerSoldiers.Length)
            {
                // every shot was team hit. This soldier cannot make a move
                //shootInteraction.Add(0);
            }

        }


        int soldier_id = Random.Range(0, enemySoldiers.Length);
        return enemySoldiers[soldier_id];
    }

    private void DoEnemyTurn()
    {
        Debug.Log("Its enemy turn :)");

        //Random choose enemy soldier and target
        //TODO: More intelligent solution

        var enemy = CountTurnEffectiveness();

        var playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");
        int soldier_id = Random.Range(0, playerSoldiers.Length);
        var player = playerSoldiers[soldier_id];


        // Find weapon of chosen enemy soldier


        var weapon = enemy.transform.Find("Weapon");
        if (weapon)
        {
            weapon.LookAt(player.transform);
            weapon.localPosition = new Vector3(-0.3f, 0.5f, 0);  // Set weapon little bit higher - TODO: animation. 
            var shooter = weapon.GetComponent<Shooter>();
            shooter.Shoot();
        } else
        {
            Debug.LogError("Unable to find childred with name Weapon");
        }
        GameManager.Instance.SetPlayerTurnMode();
    }

    private class List<T1, T2, T3>
    {
    }
}
