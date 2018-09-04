using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ControllerUI {

    //WARNING: UI is still very much under construction

    //Initializations
    #region
    //UI Text Objects
    private static Canvas CanvasMainMenu;
    private static Canvas CanvasPause;
    private static Canvas CanvasLevel;
    private static Canvas CanvasDeath;
    private static Canvas CanvasVictory;
    private static bool initialized = false;

    #endregion

    // Values that will only be initializes a maximum of once per scene are initialized in Start.
    // Values that may need to be re-initialized during a scene are initialzied in InitializeValues().
    public static void InitializeUI(Canvas mainMenu, Canvas level, Canvas pause, Canvas death, Canvas victory)
    {
        if (initialized == false) {
            initialized = true;
            CanvasDeath = death;
            CanvasPause = pause;
            CanvasLevel = level;
            CanvasVictory = victory;
            CanvasMainMenu = mainMenu;
            ResetUI();
        }
        else{
            Debug.Log("Cannot InitializeUI more than once.");
        }
	}
	
	// Update is called once per frame
	public static void UpdateUI () {
    }


    public static void ResetUI()
    {
        CanvasPause.gameObject.SetActive(false);
        CanvasDeath.gameObject.SetActive(false);
        CanvasLevel.gameObject.SetActive(true);
        CanvasMainMenu.gameObject.SetActive(false);
        CanvasVictory.gameObject.SetActive(false);
    }
    

    //Canvas Methods

    public static void LevelExit(bool state)
    {
        CanvasVictory.gameObject.SetActive(state);
    }

    public static void LevelStart(bool state)
    {
        CanvasPause.gameObject.SetActive(state);
    }

    public static void PlayerDeathUI(bool state)
    {
        CanvasDeath.gameObject.SetActive(state);
        CanvasLevel.gameObject.SetActive(!state);
    }

    public static void Pause(bool state)
    {
        CanvasPause.gameObject.SetActive(state);
    }

   
}
