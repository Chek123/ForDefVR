using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : MonoBehaviour
{
    private int health = 5; // TODO: determine what type of soldier is active and load his max HP from DB
    private int hit = 5;    // TODO: get power of weapon from DB also (bullet has to know from which weapon was shooted)
    public GameObject healthBar;
    private float hit_scale;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.SetActive(true);
        hit_scale = healthBar.transform.localScale.z / health;
    }

    // Update is called once per frame
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
            else if (this.tag == "Vojak")  //TODO: Rename to PlayerSoldier tag
            {
                int currentCount = GameManager.Instance.GetPlayerSoldiersCount();
                GameManager.Instance.SetPlayerSoldiersCount(currentCount - 1);
                Debug.Log("Player soldiers count " + GameManager.Instance.GetPlayerSoldiersCount());

                GameManager.Instance.CheckWinner();
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Bullet_M4")
        {
            health -= hit;
            healthBar.transform.localScale -= new Vector3(0, 0, hit * hit_scale);
        }
    }
}
