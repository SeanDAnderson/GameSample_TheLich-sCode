using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour {

    [SerializeField] protected float volumeMod = 1;

    private void OnEnable()
    {
        GetComponent<AudioSource>().volume = ControllerAudio.Volume*volumeMod;
    }

}
