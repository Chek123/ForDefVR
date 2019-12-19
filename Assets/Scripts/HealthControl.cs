using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    [SerializeField]
    private int health;

    private Transform actualBar;
    private int maxHealth;

    public GameObject healthBar;
    private float hit_scale;
    private Animator modelAnim;

    void Start()
    {
        healthBar.SetActive(true);
        actualBar = healthBar.transform.Find("ActualBar");
        hit_scale = actualBar.localScale.z / health;
        maxHealth = health;

        modelAnim = GetComponentInParent<RandomFancyAnimationSwitch>().soldierAnimator;
    }

    public void TakeDamage(int hit)
    {
        health -= hit;
        Debug.Log(actualBar.localScale);


        //to avoid scaling bar into negative numbers 
        if (actualBar.localScale.z < hit * hit_scale)
        {
            actualBar.localScale = Vector3.zero;
        }
        else
        {
            actualBar.localScale -= new Vector3(0, 0, hit * hit_scale);
        }
        ChangeColor();

        if (hit > 0) 
        {
            modelAnim.SetTrigger("TakeDamage");
        }

        if (health <= 0)
        {
            Debug.Log("Death");
            modelAnim.SetTrigger("Death");

        }
    }

    private void ChangeColor()
    {
        if (health <= maxHealth * 0.4)
        {
            Debug.Log("Changing color");
            var barSprite = actualBar.Find("BarSprite");
            barSprite.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public int GetHealth()
    {
        return health;
    }
}
