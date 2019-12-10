using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public RaycastHit target;
    public float speed;
    [SerializeField]
    private GameObject bullet;
    public bool valueSet = false;

    private void Start()
    {
        bullet.SetActive(false);
    }


    private void OnCollisionEnter(Collision collision)
    {
     //    GameObject.Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("TU " + valueSet);
        if (valueSet)
        {
            Debug.Log("strielam");
            bullet.SetActive(true);
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, target.point, step);
        }
/*        if (transform.position == target.point)
        {
            Destroy(this);
        }
        */
    }
}
