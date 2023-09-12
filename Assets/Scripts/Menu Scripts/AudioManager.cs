using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    /*
    Changes the volume of the game according to the volume slider in the game menu.
    */
    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
    }
}
