using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The trigger for the LevelExit objects.
//Used to detect a player collision with an exit and update the game state.
//Currently the same as winning since the game has only one big level.
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
