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
    public enum GameState { Start, Play, Pause, LevelEnd, Death }

    [Header("Game State")]
    [SerializeField] protected int levelCurrent = 0;
    [SerializeField] protected Difficulty difficulty = Difficulty.Medium;
    [SerializeField] protected GameObject controllerLevelExit;
    private static bool est = false;
    public static Difficulty CurrentDifficulty {set; get; }
    private static GameState state = GameState.Start;
    private static bool buildExists = true;
    

    [Header("Options")]
    [SerializeField] protected AudioSource audioSource;
    

    public static bool IsPaused
    {
        get {
            if (state == GameState.Pause)
            {
                return true;
            }
            else
            {
                return false;
            }
                }
    }
    public static bool IsPlay
    {
        get
        {
            if (state == GameState.Play)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static bool IsPlayerDead
    {
        get {
            if (state == GameState.Death)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    public static GameState State
    {
        get
        {
            return state;
        }
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

    //Singleton pattern
    //Ensuring only a single controller exists.
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
    
    //If this is the first and only controller, the following runtime assignments are made.
    void OnEnable()
    {
        CurrentDifficulty = difficulty;

        GetGameController = this.gameObject.GetComponent<ControllerGame> ();

        ControllerUI.InitializeUI(uiMainMenu, uiLevel, uiPause, uiDeath, uiVictory);

        SceneManager.sceneLoaded += OnSceneLoaded;

        InitializeAudio();
    }

    //These assignments are less time sensitive and handles at the Start cycle.
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

    //Resets all values to their level defaults when a new scene (i.e. game level) is loaded.
    //Because the current game contains only a single scene, this is used to reload after victory or death.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        levelCurrent = scene.buildIndex;
        ResetValues();
    }

    //The Update functions are called every game cycle. They are broken into seperate methods for readability.
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
        //Determining if the game should be paused when the Pause button is pressed.
        //Only a press triggers this, holding or lifiting the button do nothing.
        if (Input.GetButtonDown("Pause"))
        {
            switch (state)
            {
                case GameState.Play:
                    state = GameState.Pause;
                    break;
                case GameState.Start:
                case GameState.Pause:
                    state = GameState.Play;
                    break;
            }
        }

        //Setting TimeScale based on game state. All physics operations are controlled by the TimeScale.
        switch (state)
        {
            case GameState.Play:
                Time.timeScale = 1;
                break;
            case GameState.Death:
                Time.timeScale = .5f;
                break;
            case GameState.Pause:
            case GameState.Start:
            case GameState.LevelEnd:
                Time.timeScale = 0;
                break;
        }
    }

    //Resets Per Level Values to Default
    private void ResetValues()
    {
        state = GameState.Start;
        buildExists = true;
        ControllerUI.ResetUI();
    }

    //Effects triggered when the player drops to 0 health or less.
    public static void PlayerDead(bool isDead)
    {
        if (isDead)
        {
            state = GameState.Death;
        }
    }
    
    //Called to set the state to LevelEnd
    public void LevelExit()
    {
        state = GameState.LevelEnd;
    }

    //Returns a float value for the current difficulty setting for difficulty-dependent calculations.
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
        Debug.Log("Error, invalid difficulty. Setting to Medium (3).");
        return 3;
    }

    //Updates the game difficulty.
    //Called by a Dropdown Menu in the UI.
    public void UpdateDifficulty(Dropdown dropdown)
    {
        CurrentDifficulty = (Difficulty)dropdown.value; 
    }

    //Sets the game mode to play. 
    //Checks if the Exits have been built for the level, and builds them if they have not.
    public void Play()
    {
        if (buildExists)
        {
            controllerLevelExit.GetComponent<ControllerLevelExit>().BuildExits();
        }
        state = GameState.Play;
    }
    
    //Reloads teh current level
    public void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Loats a designated level (Not currently used as the game ended with a single level).
    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    //Exits the game.
    //Included in the controller to ensure that no components can call the quit directly in case other
    //opertaions need to be performed.
    public void QuitGame()
    {
        Application.Quit();
    }

    //Initializes the Audio Controller and connects it with the music audio source.
    public void InitializeAudio()
    {
        ControllerAudio.SetMusicSource(audioSource);
    }
}
