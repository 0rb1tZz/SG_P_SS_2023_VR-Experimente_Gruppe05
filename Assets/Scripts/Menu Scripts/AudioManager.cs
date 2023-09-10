using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;

    void Start(){
        AudioListener.volume = 0.5f;
    }

    public void ChangeVolume(){
        AudioListener.volume = volumeSlider.value;
    }
}
