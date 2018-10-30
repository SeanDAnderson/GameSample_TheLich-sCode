using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HEALTH BAR
//Updates a slider to show the Player's current health in the Level UI.
public class HealthBar : MonoBehaviour {

    //Declarations & Initializations
    #region
    [SerializeField] protected Slider healthbar;
    private float healthMax;
    #endregion

    //Getting the Text that the script is running on.
    private void Start()
    {
        healthbar.maxValue = ControllerPlayer.PlayerHealth;
        healthbar.value = ControllerPlayer.PlayerHealth;
    }

    //Only updates teh value during physics (fixed) updates becasue objects only move during physics updates
    //so the value should not change at other times.
    private void FixedUpdate()
    {
        healthbar.value = ControllerPlayer.PlayerHealth;
    }

}
