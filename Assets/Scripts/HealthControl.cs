using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class HealthControl : MonoBehaviour
{
    [SerializeField]
    private int health;

    private Transform actualBar;
    private int maxHealth;
    private bool animate = false;
    private int maxAnimationLength;
    private int currentAnimationLength;

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

    private void Update()
    {
        if (animate)
        {
            if (currentAnimationLength <= maxAnimationLength)
            {
                actualBar.localScale -= new Vector3(0, 0, 0.01f);
                ChangeColor();
                currentAnimationLength += 1;
            }
            else
            {
                animate = false;
            }
        }
    }

    public void TakeDamage(int hit)
    {
        int newHealth = health - hit;

        // to avoid healing soldier over maximum hp

        if (newHealth > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = newHealth;
        }

        animate = true;
        currentAnimationLength = 0;

        //to avoid scaling bar into negative numbers 

        if (actualBar.localScale.z < hit * hit_scale)
        {
            maxAnimationLength = (int)(actualBar.localScale.z * 100);
        }
        else
        {
            maxAnimationLength = (int)(hit * hit_scale * 100);
        }


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
        if (actualBar.localScale.z <= 0.4)
        {
            var barSprite = actualBar.Find("BarSprite");
            barSprite.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public int GetHealth()
    {
        return health;
    }
}