using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour {

    private Slider slider;

    private void OnEnable()
    {
        slider = GetComponent<Slider>();
        slider.value = ControllerAudio.Volume;
    }

    public void UpdateVolume()
    {
        ControllerAudio.SetVolume(slider);
    }

}
