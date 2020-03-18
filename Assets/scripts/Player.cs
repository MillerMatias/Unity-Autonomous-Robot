using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    float speed = 0.08f;
    float rotSpeed = 1.5f;

    public GameObject bulletPrefab;
    public Transform spawnPoint;

    public float fireRate = 0.25f;
    public float nextFire = 0.0f;



    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(0, 0, speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(0, 0, -speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed, 0);
        }

       

            if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
            {
            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(this.transform.forward * 1000);
            nextFire = Time.time + fireRate;

            Destroy(bullet, 3.0f);
        }


    }


}
