using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Invoke("DoEnemyTurn", 2.0f);
    }

    private ShootInteraction CountTurnEffectiveness()
    {
        var enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        var playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");
        ShootInteraction randomInteraction = null;

        List<ShootInteraction> shootInteractions = new List<ShootInteraction>();

        foreach (var enemy in enemySoldiers)
        {
            var weapon = enemy.transform.Find("Weapon");
            if (weapon)                                     //baits and medics dont pass this condition => no chance to choose them.
            {                                                                                          
                foreach (var player in playerSoldiers) {
                    if (randomInteraction == null)
                    {
                        //just in case if noone can shoot player. This enemy soldier will be chosen and hit his own teammate :) What an idiot lol!
                        randomInteraction = new ShootInteraction { enemySoldier = enemy, playerSoldier = player, efficiency = 0 };
                    }
              
                    var weaponRotation = weapon.rotation;   //store old weapon rotation
                    weapon.LookAt(player.transform);
                    var shooter = weapon.GetComponent<Shooter>();
                    
                    if (!shooter.TeamHit())   // enemy soldier didnt hit teammate. Valuable player :)      
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

        if (shootInteractions.Count == 0)   //team hit
        {
            return randomInteraction;
        }
        else
        {
            shootInteractions.Sort((a, b) => a.efficiency.CompareTo(b.efficiency));  // sort interaction ascending based on efficiency 
            var random_interaction = UnityEngine.Random.Range(0, Math.Min(2, shootInteractions.Count));
            return shootInteractions[random_interaction];
        }
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
            weapon.localPosition = new Vector3(-0.3f, 0.5f, 0);  // Set weapon little bit higher - TODO: animation. 
            weapon.LookAt(player.transform);
            var shooter = weapon.GetComponent<Shooter>();
            shooter.Shoot();
            shooter.ResetWeapon();
        }
        else
        {
            Debug.Log("Medic turn?");
            // ITS MEDIC TURN
            //TODO: Add behaviour for medic here.
        }
        GameManager.Instance.SetPlayerTurnMode();
    }
}
