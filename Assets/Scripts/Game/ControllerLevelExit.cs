﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//LEVEL EXIT
//Triggers the LevelExit method in GameController when the player enters the collider.
public class ControllerLevelExit : MonoBehaviour {

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
        else
        {
            exitPoint = gameObject;
        }

        
    }

    

    private void ActivateAll()
    {
        for ( int i = 0; i < exitPoints.Length; i++ )
        {
            Instantiate(exitPoint, exitPoints[i].transform.position, exitPoints[i].transform.rotation);
        }
    }

    private void ActivateSome()
    {
        for (int i = 0; i < exitPoints.Length/2; i++)
        {
            int rand = Random.Range(0, exitPoints.Length-1);
            Instantiate(exitPoint, exitPoints[rand].transform.position, exitPoints[rand].transform.rotation);
            if (exitPoints[rand + 1] != null)
                exitPoints[rand] = exitPoints[rand + 1];
            else
            {
                exitPoints[rand] = exitPoints[rand - 1];
            }
        }
    }

    private void ActivateOne()
    {
        int rand = Random.Range(0, exitPoints.Length - 1);
        Instantiate(exitPoint, exitPoints[rand].transform.position, exitPoints[rand].transform.rotation);
    }

}