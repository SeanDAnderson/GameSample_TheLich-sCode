using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour {

    [SerializeField]protected ControllerGame controllerGame;

    //OnTriggerEnter
    //When the player enters the attached collider LevelExit is run by the GameController.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            controllerGame.LevelExit();
        }

    }
}
