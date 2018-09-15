using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SPAWNERCONTROLLER
//The controller class for Spawner type Bugs.
//The Spawner Bug is a stationary mob (I know) that spawns another type of bug, rotates to look at the player and fires projectiles at the player.
//The Spawner is immune to damage until it becomes ISOLATED. A player can ISOLATE a Spawner Bug by reducing it's speed to zero, either with direct
//speed reduction abilities or very slowly through conventional attacks.
//The Spawner Bug's speed determines the spawn rate and fire rate for the mob.
public class ControllerMobSpawner : MonoBehaviour, IVulnerable, ISpeedMod {

    //DECLARATIONS & INITIALIZATIONS
    #region
        //TESTING
    private bool isActive = false;

    [Header("Health & Damage")]
    [SerializeField] protected float health = 10;
    [SerializeField] protected float healthMax = 10;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected float attackTimer = 1f;
    [SerializeField] protected bool isIsolated = false;
    [SerializeField] protected Color colorDefault = Color.white;
    [SerializeField] protected Color isolationColor = Color.red;
    [SerializeField] protected Transform isolationTransform;
    [SerializeField] protected float isolationTimerMax = 10f;
    [SerializeField] protected GameObject deathEmission;
    private float isolationTimer;

    //IVulnerable Properties
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    public float HealthMax
    {
        get
        {
            return healthMax;
        }
        set
        {
            healthMax = value;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    [Space]
    [Header("Speed & Physics")]
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float speedDefault = 5f;
    [SerializeField] protected float speedMax = 30f;
    [SerializeField] protected float speedMin = 0f;
    [SerializeField][Range(.01f, 1f)] protected float speedRevocery = .1f;
    [SerializeField] [Range(.01f, 1f)] protected float rotationSpeed = .5f;
    private Vector2 facing;
    private Rigidbody2D body;
    private Vector2 location;

    //ISpeedMod Properties
    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }
    public float SpeedDefault
    {
        get
        {
            return speedDefault;
        }
        set
        {
            speedDefault = value;
        }
    }
    public float SpeedMax
    {
        get
        {
            return speedMax;
        }
        set
        {
            speedMax = value;
        }
    }
    public float SpeedMin
    {
        get
        {
            return speedMin;
        }
        set
        {
            speedMin = value;
        }
    }

    [Space]
    [Header("Spawning")]
    [SerializeField] protected GameObject spawn;
    [SerializeField] protected float spawnTimer = 0f;
    [SerializeField][Range(1,100)] protected float spawnSpeed = 50f;
    [SerializeField] protected bool canSpawn;

    //Rendering
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer isolationSpriteRenderer;

    #endregion


    //Any variables that may have to be reinitialized during play are separated into a private InitializeValues() method.
    void Start () {

        health = healthMax * ControllerGame.DifficultySetting();

        spriteRenderer = GetComponent<SpriteRenderer>();
        isolationSpriteRenderer = isolationTransform.GetComponent<SpriteRenderer>();
        isolationSpriteRenderer.enabled = false;

        //Destroys itself if there is nothing to spawn.
        if (spawn == null)
        {
            Destroy(gameObject);
            Debug.Log("Nothing to spawn. BALEETED!");
        }
	}
	
	//Update is divided into private methods
    //Most methods depend on current pause state.
	void Update () {

        //Checks for activation each tick (So that it does not keep spawning deactivated enemies when the player walks away)
        isActive = ControllerMob.ActivationCheck(gameObject);

        UpdateIsolation();

        if ((isActive) && (!ControllerGame.IsPaused))
        {

            

            if (isIsolated == false)
            {
                UpdateRotation();

                UpdateSpawn();

                //UpdateAttack();
            }
        }
        
	}

    //Attack TODO
    //WILL Fire projectiles based on the Spawner Bug's current facing based on it's speed.
    private void UpdateAttack()
    {
        //Attack on timer cooldown
    }


    //UpdateIsolation
    //Checks the current isIsolated state, tracks the isolationTimer and updates the isIsolated state using IsolationToggle when needed. 
    //bool  isIsolated      Whether the Spawner is in an isolated state
    //float speed           ISpeedMod speed value, determines how quickly the Spawner Bug spawns mobs and fires projectiles, damaged by player attacks
    //float isolationTimer  How long the ISOLATED state has remaining
    private void UpdateIsolation()
    {
        /*ISOLATION CHECK
         * If the spawner's speed is reduced below 0 it becomes isolated (and vulnerable)
         */
        if ((speed <= 0) && (isIsolated == false))
        {
            IsolattionToggle(true);
        }

        /*ISOLATION TIMER
         * Timer coundts down the duration of the isolation (only when Isolation is in effect)
         * When timer hits 0, isolation ends. Also makes sure that 
         */
        if (isolationTimer > 0)
        {
            isolationTimer -= Time.deltaTime;
        }

        if ((isIsolated == true) && (isolationTimer <= 0))
        {
            IsolattionToggle(false);
        }

        //REMOVED Speed Regeneration. It was invisible to the player and just complicated the mechanics without noticable benefit.
      
    }

    //UpdateRotation
    //Points the Spawner Bug at the player's location
    //private   Vector2   facing                            The Vector2 representing the direction the mob should face
    //static    Vector2   PlayerController.PlayerLocation   The players last recorded location
    private void UpdateRotation()
    {
        facing = ControllerPlayer.PlayerLocation - new Vector2(transform.position.x, transform.position.y);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg - 90, Vector3.forward), rotationSpeed);
    }

    //UpdateSpawn
    //Spawning is switched on and off with canSpawn
    //Currently the code assumes the Spawner can only create Seeker Bugs.
    //Spawns (Speed) creatures per second at spawnSpeed 100
    //MobController handles the spawning 
    //bool      canSpawn        Switches spawning on and off
    //float     speed           How quickly mobs spawn. Multiplies the effect of spawnSpeed.
    //flaot     spawnSpeed      How quickly mobs spawn. 1 per second at 100 - 1 per 100 seconds at 1. 
    //float     spanwTimer      How long remaining until a new mob is spawned
    private void UpdateSpawn()
    {
        if (canSpawn == true)
        {
            
                
            spawnTimer -= speed * (.01f * spawnSpeed) * Time.deltaTime;

            if (spawnTimer <= 0)
            {
                spawnTimer = 1;
                ControllerMob.AddSeeker(spawn, gameObject);
            }
        }

    }

    //IsolationToggle
    //Handles the ISOLATION activation/deactivation mechanic for the Spanwer Bug
    //TRUE      Sets all values to the ISOLATED state
    //FALSE     Sets all values to their default, non-ISOLATED state
    private void IsolattionToggle(bool state)
    {
        if (state == true)
        {
            isIsolated = true;
            isolationTimer = isolationTimerMax;
            isolationSpriteRenderer.enabled = true;
            canSpawn = false;
            spriteRenderer.color = isolationColor;
        }
        else
        {
            isIsolated = false;
            SpeedReset();
            isolationSpriteRenderer.enabled = false;
            canSpawn = true;
            spriteRenderer.color = colorDefault;
        }

    }

    //IVulnerable Functions
    #region
        //Heal & KIll are IVulnerable Standard using DamageUtilities
        //Injure slows the Spawner until it becomes ISOLATED then deals damage as IVulnerable standard with DamageUtilities
    public void Heal(float amount)
    {
        DamageUtilities.Heal(this, amount);
    }

    public void Injure(float amount)
    {
        if (isIsolated == true)
        {
            DamageUtilities.Injure(this, amount);
        }
        else
        {
            SpeedDecrease(amount / 10);
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
        if (deathEmission != null)
        {
            Instantiate(deathEmission, transform.position, transform.rotation);
        }
    }
    #endregion

        //ISpeedMod Functions
    #region
        //SpeedDecrease and SpeedReset are ISpeedMod standard using DamageUtilities
        //SpeedIncrease has no functionality
    public void SpeedDecrease(float amount)
    {
        if (speed >= 0)
        {
            DamageUtilities.SpeedDecrease(this, amount);
        }

    }

    public void SpeedReset()
    {
        DamageUtilities.SpeedReset(this);
    }

    public void SpeedIncrease(float amount)
    {
        //DO NOTHING (right now)
    }
    #endregion
}
