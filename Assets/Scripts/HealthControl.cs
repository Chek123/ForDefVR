using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    [SerializeField]
    private int health;

    [SerializeField]
    private AudioSource deathSound;

    private float animationSpeed = 1.5f;

    private Transform actualBar;
    private int maxHealth;
    private bool animate = false;
    private int expectedHealthBarScale;

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
            if ((actualBar.localScale.z * 100) >= expectedHealthBarScale)
            {
                actualBar.localScale -= new Vector3(0, 0, animationSpeed * Time.deltaTime);
                ChangeColor();
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

        //heal
        if(hit < 0)
        {
            actualBar.localScale = Vector3.one;
            health = maxHealth;
            ChangeColor();
        }

        animate = true;

        //to avoid scaling bar into negative numbers 

        if (actualBar.localScale.z < hit * hit_scale)
        {
            expectedHealthBarScale = 0;
        }
        else
        {
            expectedHealthBarScale = (int)((actualBar.localScale.z * 100) - (hit * hit_scale * 100));
        }


        if (hit > 0) 
        {
            modelAnim.SetTrigger("TakeDamage");
            deathSound.Play(0);
        }

        if (health <= 0)
        {
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