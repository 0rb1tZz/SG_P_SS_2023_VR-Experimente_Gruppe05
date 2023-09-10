using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
    public CharacterController cc;
    public AudioSource audioSource;
    private float defaultVolume;
    private float defaultPitch;

    void Start()
    {
        defaultVolume = audioSource.volume;
        defaultPitch = audioSource.pitch;
    }
    // Update is called once per frame
    void Update()
    {
         if (cc.isGrounded == true && cc.velocity.magnitude > 2f && audioSource.isPlaying == false)
        {
            audioSource.volume = defaultVolume * Random.Range(0.8f, 1);
            audioSource.pitch = defaultPitch * Random.Range(0.9f, 1.05f);
            audioSource.Play();
        }
    }
}
