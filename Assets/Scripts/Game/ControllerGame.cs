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
    [SerializeField] protected bool audioEnabled = true;
    [SerializeField] [Range(0, 1)] protected float audioVolume = .5f;
    [SerializeField] protected bool musiceEnabled = true;
    [SerializeField] [Range(0, 1)] protected float musicVolume = 1f;
    [SerializeField] protected bool soundFXEnabled = true;
    [SerializeField] [Range(0, 1)] protected float soundFXVolume = 1f;


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

    public void LevelExit()
    {
        state = GameState.LevelEnd;
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
        Debug.Log("Error, invalid difficulty. Setting to Medium (3).");
        return 3;
    }

    public void UpdateDifficulty(Dropdown dropdown)
    {
        CurrentDifficulty = (Difficulty)dropdown.value; 
    }

    public void Play()
    {
        if (buildExists)
        {
            controllerLevelExit.GetComponent<ControllerLevelExit>().BuildExits();
        }
        state = GameState.Play;
    }
    
    public void ReloadLevel()
    {
        ControllerGame.state = GameState.Play;
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

    public void UpdateAudio(Slider slider)
    {
        audioVolume = slider.value/100;
        audioSource.volume = audioVolume;
    }
}
