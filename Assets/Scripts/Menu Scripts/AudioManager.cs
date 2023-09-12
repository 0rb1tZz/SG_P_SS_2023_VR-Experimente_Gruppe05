using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A script attached to a slider, to change the ingame volume
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider; // The slider specifying the audio level

    /// <summary>
    /// A function to change the volume of the audio listener to match the value specified by the slider
    /// </summary>
    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
    }
}
