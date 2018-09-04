using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//HEALTH BAR
//Creates and updates a string to show the Player's current health in the Level UI.
public class HealthBar : MonoBehaviour {

    //Declarations & Initializations
    #region
    private Text Healthbar;
    private float health;
    private string healthString;
    #endregion

    //Getting the Text that the script is running on.
    private void OnEnable()
    {
        Healthbar = gameObject.GetComponent<Text>(); 
    }

    //UPDATE
    //Checks to see if the player's health has changed. If it has, it rebuilds the string and assigns it to the UI text.
    void Update () {
		if (health != ControllerPlayer.PlayerHealth)
        {
            health = ControllerPlayer.PlayerHealth;
            healthString = "HEALTH: ";

            for (float i = health; i>0; i--)
            {
                healthString = healthString + "|";
            }

            Healthbar.text = healthString;
        }
	}
}
