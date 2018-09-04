using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LEVEL EXIT
//Triggers the LevelExit method in GameController when the player enters the collider.
public class LevelExit : MonoBehaviour {

    //Declarations & Initializations
    #region
    private ControllerGame gameController;
    [SerializeField] protected GameObject[] exitPoints;
    [SerializeField] protected GameObject exitPoint;
    #endregion

    //Attempts to get the GameController if no game controller is assigned.
    private void Start()
    {
        if (gameController == null)
        {
            gameController = ControllerGame.GetGameController;
        }

        //Setting Random Level Exit Location
        if (exitPoints.Length > 0)
        {
            exitPoint = exitPoints[Random.Range(0, exitPoints.Length)];
            gameObject.transform.position = exitPoint.transform.position;
        }
        else
        {
            exitPoint = gameObject;
        }

        
    }

    //OnTriggerEnter
    //When the player enters the attached collider LevelExit is run by the GameController.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameController.LevelExit();
        }
        
    }
}
