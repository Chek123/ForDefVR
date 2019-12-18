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

    void Update()
    {
        if (health <= 0)
        {

            if (this.tag == "EnemySoldier")
            {
                int currentCount = GameManager.Instance.GetEnemySoldiersCount();
                GameManager.Instance.SetEnemySoldiersCount(currentCount - 1);
                Debug.Log("Enemy soldiers count " + GameManager.Instance.GetEnemySoldiersCount());

                GameManager.Instance.CheckWinner();
            }
            else if (this.tag == "Vojak")
            {
                int currentCount = GameManager.Instance.GetPlayerSoldiersCount();
                GameManager.Instance.SetPlayerSoldiersCount(currentCount - 1);
                Debug.Log("Player soldiers count " + GameManager.Instance.GetPlayerSoldiersCount());

                GameManager.Instance.CheckWinner();
            }
            Destroy(gameObject);
        }
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
    }
}
