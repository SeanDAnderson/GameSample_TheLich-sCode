using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//CONTROLLER: GAME
//Oversees overall game state and mechanics.
//Especially Pause and Death mechanics.
public class ControllerGame : MonoBehaviour {

    //Declarations and Initializations
    #region

    public enum Difficulty { VeryEasy, Easy, Medium, Hard, Impossible }

    [Header("Game State")]
    [SerializeField] protected int levelCurrent = 0;
    [SerializeField] protected static bool isPaused = false;
    [SerializeField] protected static bool isLevelExit = false;
    [SerializeField] protected static bool isPlayerDead = false;
    [SerializeField] protected Difficulty difficulty = Difficulty.Medium;
    private static bool est = false;
    public static Difficulty CurrentDifficulty {set; get; }


    public static bool IsPaused
    {
        get { return isPaused; }
    }
    public static bool IsLevelExit
    {
        get { return isLevelExit; }
    }
    public static bool IsPlayerDead
    {
        get { return isPlayerDead; }
    }

    public static bool IsQuitting;
    public static ControllerGame GetGameController;

    [Space]
    [Header("User Interface")]
    [SerializeField] protected Canvas uiLevel;
    [SerializeField] protected Canvas uiPause;
    [SerializeField] protected Canvas uiMainMenu;
    [SerializeField] protected Canvas uiDeath;
    [SerializeField] protected Canvas uiVictory;

    #endregion

    void Awake()
    {
        if (!est)
        {
            DontDestroyOnLoad(gameObject);
            est = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    void OnEnable()
    {
        CurrentDifficulty = difficulty;

        GetGameController = this.gameObject.GetComponent<ControllerGame> ();

        ControllerUI.InitializeUI(uiMainMenu, uiLevel, uiPause, uiDeath, uiVictory);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        //Collision Igores
        //Player & PlayerMissile
        Physics2D.IgnoreLayerCollision(8, 11);
        //PlayerMissile & Player Missile
        Physics2D.IgnoreLayerCollision(11, 11);
        //Ghosts & Ground
        Physics2D.IgnoreLayerCollision(9, 13);




    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCurrent = scene.buildIndex;
        ResetValues();
    }

    void Update () {
        UpdateGameSpeed();
        ControllerInput.UpdateInput();
        ControllerUI.UpdateUI();
    }


    
    //When the pause button (defaults to ESC) is pressed the pause state toggles.
    //While paused Time.timescale is set to 0 to stop Unity physics from running.
    //Most Mobs check IsPaused in Update.
    private void UpdateGameSpeed()
    {
        if (Input.GetButtonDown("Pause"))
        {
            isPaused = !IsPaused;
            Debug.Log("Pause Toggle");
            ControllerUI.Pause(isPaused);
        }


        if (IsPaused == true)
        {
            Time.timeScale = 0;
        }
        else if (isPlayerDead == true)
        {
            Time.timeScale = .5f;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    //Resets Per Level Values to Default

        private void ResetValues()
    {
        isPaused = false;
        isPlayerDead = false;
        ControllerUI.ResetUI();
    }

    //Effects triggered when the player drops to 0 health or less.
    public static void PlayerDead(bool state)
    {
        isPlayerDead = state;
        ControllerUI.PlayerDeathUI(state);
    }

    public void LevelExit()
    {
        isPaused = true;
        ControllerUI.LevelExit(true);
    }

    public static float DifficultySetting()
    {
        switch (CurrentDifficulty)
        {
            case Difficulty.VeryEasy:
                return 1;
            case Difficulty.Easy:
                return 2;
            case Difficulty.Medium:
                return 3;
            case Difficulty.Hard:
                return 5;
            case Difficulty.Impossible:
                return 10;
        }
        Debug.Log("Error, invalid difficulty. Setting to 0.");
        return 0;
    }

    public void ReloadLevel()
    {
        ControllerGame.PlayerDead(false);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
