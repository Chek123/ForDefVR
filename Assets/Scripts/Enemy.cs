using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ShootInteraction
{
    public GameObject enemySoldier { get; set; }
    public GameObject playerSoldier { get; set; }
    public double efficiency { get; set; }

    public void IsEmpty()
    {

    }
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

        foreach (var enemy in enemySoldiers)
        {
            var weapon = enemy.transform.Find("Weapon");
            if (weapon)                                     //Baits and medics dont pass this condition => no chance to choose them.
            {
                foreach (var player in playerSoldiers) {
                    var weaponRotation = weapon.rotation;   //store old weapon rotation
                    weapon.LookAt(player.transform);
                    var shooter = weapon.GetComponent<Shooter>();
                    
                    if (!shooter.TeamHit())               
                    {

                        // Atribute 1: test raycast shoot to each player soldier
                        // if any player is hit, effectiveness is based on (maxSoldierHP (per game) - current HP of hit soldier) multiplied by factor 2

                        var HP = player.GetComponent<HealthControl>().health;    
                        double efficiency = (GameManager.Instance.GetMaxSoldierHP() - HP) * 2;   

                        // Atribute 2: find out hp of current enemy soldier
                        // Effectiveneess is based on (maxSoldierHP (per game) - current HP of enemy soldier) multiplied by factor 1.5

                        HP = enemy.GetComponent<HealthControl>().health;                    //TODO: do get function (during health bar refactor)
                        efficiency += (GameManager.Instance.GetMaxSoldierHP() - HP) * 1.5;


                        // Atribute 3: find out weapon damage of current enemy soldier
                        // Damage of weapon is added to current efficiency 
                        efficiency += weapon.GetComponent<Shooter>().GetWeaponDamage();

                        // Store <EnemySoldier, PlayerSoldier, efficiency> interaction
                        shootInteractions.Add(new ShootInteraction { enemySoldier = enemy, playerSoldier = player, efficiency = efficiency});
                    }
                    weapon.rotation = weaponRotation;       //set weapon rotation to old state
                }
            }
        }

        // Sort interaction ascending based on efficiency 
        shootInteractions.Sort((a, b) => a.efficiency.CompareTo(b.efficiency));

        // Just debug print
        foreach (var interaction in shootInteractions)
        {
            Debug.Log(interaction.enemySoldier + "," + interaction.playerSoldier + "," + interaction.efficiency);
        }

        // Choose random interaction from first 2 results
        var random_interaction = UnityEngine.Random.Range(0, 1);
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
        weapon.localPosition = new Vector3(-0.3f, 0.5f, 0);  // Set weapon little bit higher - TODO: animation. 
        weapon.LookAt(player.transform);
        var shooter = weapon.GetComponent<Shooter>();
        shooter.Shoot();
        GameManager.Instance.SetPlayerTurnMode();
    }
}
