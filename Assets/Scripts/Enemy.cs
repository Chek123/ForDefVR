﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public void EnemyController()
    {
        Invoke("DoEnemyTurn", 5.0f);
    }

    private void DoEnemyTurn()
    {
        Debug.Log("Its enemy turn :)");

        //Random choose enemy soldier and target
        //TODO: More intelligent solution

        var enemySoldiers = GameObject.FindGameObjectsWithTag("EnemySoldier");
        int soldier_id = Random.Range(0, enemySoldiers.Length);
        var enemy = enemySoldiers[soldier_id];

        var playerSoldiers = GameObject.FindGameObjectsWithTag("Vojak");
        soldier_id = Random.Range(0, playerSoldiers.Length);
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
