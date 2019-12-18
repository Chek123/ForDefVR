using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShootInteraction
{
    public GameObject enemySoldier { get; set; }
    public GameObject playerSoldier { get; set; }
    public double efficiency { get; set; }
}

public class Enemy : MonoBehaviour
{
    public void EnemyController()
    {
        Invoke("DoEnemyTurn", 5.0f);
    }

    private ShootInteraction CountTurnEffectiveness()
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

        //2. step 
        // Get HP of each enemy soldier and calculate efficient parameter.

        foreach (var interaction in shootInteractions)
        {
            var enemyHP = interaction.enemySoldier.GetComponent<HealthControl>().health;  //do get function
            var new_efficiency = (GameManager.Instance.GetMaxSoldierHP() - enemyHP) * 1.5;
            interaction.efficiency += new_efficiency; 
        }

        //3. step
        //get weapon power of enemy soldier

        foreach (var interaction in shootInteractions)
        {
            var enemy = interaction.enemySoldier;
            var weapon = enemy.transform.Find("Weapon");  //tu uz musia mat weapon, inak by to nepreslo stepom 1.
            var weaponDamage = weapon.GetComponent<Shooter>().GetWeaponDamage();
            interaction.efficiency += weaponDamage;
        }

        // sort all results
        shootInteractions.Sort((a, b) => a.efficiency.CompareTo(b.efficiency));

        foreach (var interaction in shootInteractions)
        {
            Debug.Log(interaction.enemySoldier + "," + interaction.playerSoldier + "," + interaction.efficiency);
        }


        //random z prvych 3 vysledkov
        var random_interaction = UnityEngine.Random.Range(0, 2);
        return shootInteractions[random_interaction];
    }

    private void DoEnemyTurn()
    {
        Debug.Log("Its enemy turn :)");

        var interaction = CountTurnEffectiveness();
        var enemy = interaction.enemySoldier;
        var player = interaction.playerSoldier;

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
