using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Generates the level exits randomly depenending on the game difficulty.
//At the easiest difficulties (VeryEasy, Easy) all exits are open.
//At the intermediate difficulty (Medium) half of the exits, rounding up, are open.
//At the highest difficulties (Hard, Impossible) only one exit is open.
//The possible level exits are empty game objects stored in an array in the Unity Editor.
//The Controller creates the LevelExit objects at the transform positions of those objects.
public class ControllerLevelExit : MonoBehaviour {

    //Declarations & Initializations
    #region
    private ControllerGame gameController;
    [SerializeField] protected GameObject[] exitPoints;
    private GameObject[] exitPointsRemaining;
    [SerializeField] protected GameObject exitPoint;
    #endregion

    //Attempts to get the GameController if no game controller is assigned.
    public void BuildExits()
    {
        exitPointsRemaining = exitPoints;
        if (gameController == null)
        {
            gameController = ControllerGame.GetGameController;
        }

        //Setting Random Level Exit Location
        if (exitPoints.Length > 0)
        {
            switch (ControllerGame.CurrentDifficulty)
            {
                case ControllerGame.Difficulty.VeryEasy:
                case ControllerGame.Difficulty.Easy:
                    ActivateAll();
                    break;
                case ControllerGame.Difficulty.Medium:
                    ActivateSome();
                    break;
                case ControllerGame.Difficulty.Hard:
                case ControllerGame.Difficulty.Impossible:
                    ActivateOne();
                    break;
            }


            
        }
        //If the array of exits is empty a generic exit is opened.
        else
        {
            exitPoint = gameObject;
        }
    }

    
    //Creates a LevelExit object at every possible exit point.
    private void ActivateAll()
    {
        for ( int i = 0; i < exitPoints.Length; i++ )
        {
            Instantiate(exitPoint, exitPoints[i].transform.position, exitPoints[i].transform.rotation);
        }
    }

    //Creates a LevelExit object and half of the possible exit points, rounded up.
    private void ActivateSome()
    {

        for (int i = 0; i < exitPoints.Length/2; i++)
        {
            int rand = Random.Range(0, exitPoints.Length-1);
            Instantiate(exitPoint, exitPointsRemaining[rand].transform.position, exitPointsRemaining[rand].transform.rotation);
            if (exitPointsRemaining[rand + 1] != null)
                exitPointsRemaining[rand] = exitPointsRemaining[rand + 1];
            else
            {
                exitPointsRemaining[rand] = exitPointsRemaining[rand - 1];
            }
        }
    }

    //Creates a single LevelExit at one of the possible exit points.
    private void ActivateOne()
    {
        int rand = Random.Range(0, exitPoints.Length - 1);
        Instantiate(exitPoint, exitPoints[rand].transform.position, exitPoints[rand].transform.rotation);
    }

}
