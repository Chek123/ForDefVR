using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShootInteraction
{
    public GameObject enemySoldier { get; set; }
    public GameObject playerSoldier { get; set; }
    public int efficiency { get; set; }
}

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

        List<ShootInteraction> shootInteractions = new List<ShootInteraction>();

        // 1. step
        // Every enemy soldier (with weapon) tries to shoot on every player soldier.
        foreach (var enemy in enemySoldiers)
        {
            var weapon = enemy.transform.Find("Weapon");
            if (weapon)
            {
                foreach (var player in playerSoldiers) {
                    var weaponRotation = weapon.rotation;  //store old weapon rotation
                    weapon.LookAt(player.transform);
                    var shooter = weapon.GetComponent<Shooter>();

                    if (!shooter.TestShoot())
                    { 
                        var hitHP = player.GetComponent<HealthControl>().health;
                        var efficiency = (GameManager.Instance.GetMaxSoldierHP() - hitHP) * 2;
                        //shootInteractions.Add(new Tuple<GameObject, GameObject, int>(enemy, player, efficiency));
                        shootInteractions.Add(new ShootInteraction { enemySoldier = enemy, playerSoldier = player, efficiency = efficiency});
                    }
                    weapon.rotation = weaponRotation;   //set weapon rotation to old state
                }
            }

        }

        // DEBUG
        foreach(var interaction in shootInteractions)
        {
            Debug.Log(interaction.enemySoldier + "," + interaction.playerSoldier + "," + interaction.efficiency);
        }

        //2. step 
        // Get HP of each enemy soldier and calculate efficient parameter.

        foreach (var interaction in shootInteractions)
        {
            //var enemy = interaction.Item1.GetComponent<HealthControl>().health;  //do get function

        }


        //Debug.Log(shootInteractions);


        int soldier_id = UnityEngine.Random.Range(0, enemySoldiers.Length);
        return enemySoldiers[soldier_id];
    }

    private void DoEnemyTurn()
    {
        Debug.Log("Its enemy turn :)");

        //Random choose enemy soldier and target
        //TODO: More intelligent solution

        var enemy = CountTurnEffectiveness();

        var playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");
        int soldier_id = UnityEngine.Random.Range(0, playerSoldiers.Length);
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
}
