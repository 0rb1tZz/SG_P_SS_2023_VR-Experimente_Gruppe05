using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GOT HIT!");
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "LaserBeam")
        {
            Laser_old laser = collision.gameObject.transform.parent.gameObject.GetComponent<Laser_old>();
            laser.laserBeam.UpdateLaser();
        }
    }
}
