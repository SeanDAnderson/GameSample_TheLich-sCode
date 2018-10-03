using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ControllerUI {

    //WARNING: UI is still very much under construction

    //Initializations
    #region
    //UI Text Objects
    private static Canvas CanvasStart;
    private static Canvas CanvasPause;
    private static Canvas CanvasLevel;
    private static Canvas CanvasDeath;
    private static Canvas CanvasLevelEnd;
    private static bool initialized = false;
    private static ControllerGame.GameState uiState = ControllerGame.GameState.Start;

    #endregion

    // Values that will only be initializes a maximum of once per scene are initialized in Start.
    // Values that may need to be re-initialized during a scene are initialzied in InitializeValues().
    public static void InitializeUI(Canvas start, Canvas level, Canvas pause, Canvas death, Canvas levelEnd)
    {
        if (initialized == false) {
            initialized = true;
            CanvasDeath = death;
            CanvasPause = pause;
            CanvasLevel = level;
            CanvasLevelEnd = levelEnd;
            CanvasStart = start;
            ResetUI();
        }
        else{
            Debug.Log("Cannot InitializeUI more than once.");
        }
	}
	
	// Update is called once per frame
	public static void UpdateUI () {
        if (uiState != ControllerGame.State)
        {
            CanvasPause.gameObject.SetActive(false);
            CanvasDeath.gameObject.SetActive(false);
            CanvasLevel.gameObject.SetActive(true);
            CanvasStart.gameObject.SetActive(false);
            CanvasLevelEnd.gameObject.SetActive(false);

            switch (ControllerGame.State)
            {
                case ControllerGame.GameState.Start:
                    CanvasStart.gameObject.SetActive(true);
                    break;
                case ControllerGame.GameState.Play:
                    break;
                case ControllerGame.GameState.Death:
                    CanvasDeath.gameObject.SetActive(true);
                    break;
                case ControllerGame.GameState.LevelEnd:
                    CanvasLevelEnd.gameObject.SetActive(true);
                    break;
                case ControllerGame.GameState.Pause:
                    CanvasPause.gameObject.SetActive(true);
                    break;
            }
        }
    }


    public static void ResetUI()
    {
        CanvasPause.gameObject.SetActive(false);
        CanvasDeath.gameObject.SetActive(false);
        CanvasLevel.gameObject.SetActive(true);
        CanvasStart.gameObject.SetActive(true);
        CanvasLevelEnd.gameObject.SetActive(false);
    }
    
    //Canvas Methods DEPRECATED
    /*
    public static void LevelExit(bool state)
    {
        CanvasLevelEnd.gameObject.SetActive(state);
    }

    public static void LevelStart(bool state)
    {
        CanvasPause.gameObject.SetActive(state);
    }

    public static void PlayerDeathUI(bool state)
    {
        CanvasDeath.gameObject.SetActive(state);
    }

    public static void Pause(bool state)
    {
        CanvasPause.gameObject.SetActive(state);
    }*/

   

}
