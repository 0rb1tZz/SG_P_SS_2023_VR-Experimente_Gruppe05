using UnityEngine;

public class Barrier : MonoBehaviour
{
    /*
    When the barrier is hit by an object, the barrier collision sound is played.
    */
    void OnCollisionEnter(Collision collision){
        gameObject.GetComponent<AudioSource>().Play();
    }
}
