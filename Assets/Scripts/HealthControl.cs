using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    public int health = 5; // TODO: set this variable from scene
    public GameObject healthBar;
    private float hit_scale;

    private Animator modelAnim;

    void Start()
    {
        healthBar.SetActive(true);
        hit_scale = healthBar.transform.localScale.z / health;

        modelAnim = GetComponentInParent<RandomFancyAnimationSwitch>().soldierAnimator;
    }



/*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet_M4")
        {
            health -= hit;
            healthBar.transform.localScale -= new Vector3(0, 0, hit * hit_scale);
        }
    }
    */
    public void TakeDamage(int hit)
    {
        health -= hit;
        healthBar.transform.localScale -= new Vector3(0, 0, hit * hit_scale);

        if (hit > 0) 
        {
            modelAnim.SetTrigger("TakeDamage");
        }

        if (health <= 0)
        {
            modelAnim.SetTrigger("Death");

        }

    }
}
