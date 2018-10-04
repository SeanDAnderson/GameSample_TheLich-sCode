using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public static class ControllerAudio  {
    //Declarations & Initializations
    private static AudioSource music;
    private static float volume = .5f;
    public static float Volume { get {
            return volume;
        } }
      
    //Sets the Audiosource for Music
    //Called in ControllerGame
    public static void SetMusicSource(AudioSource audioSource)
    {
        music = audioSource;
    }

    public static void SetVolume(Slider slider)
    {
        volume = slider.value;
        music.volume = volume;
    }
    
}
