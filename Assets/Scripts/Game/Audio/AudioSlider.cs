using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Functionality for UI Sliders to contol audio volume by interacting with ControllerAudio.
public class AudioSlider : MonoBehaviour {
    //Declarations & Initializations
    private Slider slider;

    //Runtime assignments
    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.value = ControllerAudio.Volume;
    }

    //Called when the UI updates to ensure that the volume slider represents the current game audio volume.
    public void UpdateVolume()
    {
        ControllerAudio.SetVolume(slider);
    }

}
