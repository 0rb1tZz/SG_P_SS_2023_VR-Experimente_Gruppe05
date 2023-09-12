using UnityEngine;

/// <summary>
/// The script to add walking sounds to the player
/// </summary>
public class Footsteps : MonoBehaviour
{
    public CharacterController cc; // The character controller
    public AudioSource audioSource; // The audio source containing the step sound
    private float defaultVolume; // The volume of the audio source set in Unity
    private float defaultPitch; // The pitch of the audio source set in Unity

    /// <summary>
    /// The starting volume and pitch of the audio source get saved in corresponding variables
    /// </summary>
    void Start()
    {
        defaultVolume = audioSource.volume;
        defaultPitch = audioSource.pitch;
    }
    
    /// <summary>
    /// Every frame, if the player is on the ground, moving and no footstep sound is playing, a footstep sound gets played, while pitch and volume are randomized to make each step sound different
    /// </summary>
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
