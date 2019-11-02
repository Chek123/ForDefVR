﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    private int health = 5; // TODO: determine what type of soldier is active and load his max HP from DB
    private int hit = 2;    // TODO: get power of weapon from DB also (bullet has to know from which weapon was shooted)
    public Transform healthBar;
    private float hit_scale;

    // Start is called before the first frame update
    void Start()
    {
        hit_scale = healthBar.localScale.z / health;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.collider.tag);
        if (collision.collider.tag == "Bullet_M4")
        {
            health -= hit;
            healthBar.localScale -= new Vector3(0, 0, hit * hit_scale);
        }
    }
}