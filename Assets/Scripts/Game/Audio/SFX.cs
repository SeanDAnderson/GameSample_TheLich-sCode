using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Used to adjust the relative audio levels of sound effects when they are created.
//This script is attached to the object that plays the sound.
public class SFX : MonoBehaviour {
    //Declarations & Initializations
    [SerializeField] protected float volumeMod = 1;

    //Sets the audio volume of the AudioSource on the attached object when it is created.
    //If there is not audio object Unity quietly handles the null exception without incident. 
    private void OnEnable()
    {
        GetComponent<AudioSource>().volume = ControllerAudio.Volume*volumeMod;
    }

}
