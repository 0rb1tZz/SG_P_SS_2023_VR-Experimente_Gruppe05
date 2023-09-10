using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    void OnCollisionEnter(Collision collision){
        gameObject.GetComponent<AudioSource>().Play();
    }
}
