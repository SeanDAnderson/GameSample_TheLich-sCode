using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

//The general controller for game audio effects.
//Manages both volume levels and the general music audio source control.
public static class ControllerAudio  {
    //Declarations & Initializations
    #region
    private static AudioSource music;
    private static float volume = .5f;
    public static float Volume { get {
            return volume;
        } }
    #endregion

    //Sets the Audiosource for Music
    //Called in ControllerGame
    public static void SetMusicSource(AudioSource audioSource)
    {
        music = audioSource;
    }

    //Called by UI sliders to set the audio volume
    public static void SetVolume(Slider slider)
    {
        volume = slider.value;
        music.volume = volume;
    }
    
}
