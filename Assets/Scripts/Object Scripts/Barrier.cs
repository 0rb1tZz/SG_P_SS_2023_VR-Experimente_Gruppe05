using UnityEngine;

/// <summary>
/// A script attached to a barrier
/// </summary>
public class Barrier : MonoBehaviour
{
    /// <summary>
    /// When an object collides with the barrier, a collision sound is played
    /// </summary>
    /// <param name="collision">The collision between an object and the barrier</param>
    void OnCollisionEnter(Collision collision){
        gameObject.GetComponent<AudioSource>().Play();
    }
}
