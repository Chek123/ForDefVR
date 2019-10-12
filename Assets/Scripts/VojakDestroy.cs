using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VojakDestroy : MonoBehaviour
{
    public GameObject spawningController1;
    public GameObject spawningController2;

    public bool destroyed = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!destroyed)
        {
            switch (collision.gameObject.tag)
            {
                case "VojakModel1":
                    spawningController1.GetComponent<Spawning>().increaseCounter();
                    break;
                case "VojakModel2":
                    spawningController2.GetComponent<Spawning>().increaseCounter();
                    break;
            }
            Destroy(collision.gameObject);
            destroyed = true;
        }
    }

}
