using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SEEKER CONTROLLER
//The Controller Class for Seeker Bug mobs.
//These Bugs follow the player, skittering a distance, stopping, then skittering again. 
//They do not actually target the players space, but some location within 2 units of the player.
//They have differing speeds (controlled by speedOffset).
//They die and injure the player on contact.
public class ControllerMobSeeker : MonoBehaviour, IVulnerable, ISpeedMod
{
    //Declarations & Initializations
    #region
    private bool isActive = false;

    [Header("Spawning")]
    [SerializeField] public static int seekerCount = 0;
    [SerializeField] public static int seekerMax = 20;

    [Space]
    [Header("Targeting")]
    [SerializeField] protected Vector2 targetLocation = Vector2.zero;
    //[SerializeField] protected Vector2 targetLocationOffset;

    [Space]
    [Header("Health & Damage")]
    [SerializeField] protected float health = 2;
    [SerializeField] protected float healthMax = 2;
    [SerializeField] protected float damage = 1;
    [SerializeField] protected GameObject deathEmission;

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
    [Header("Movement & Physics")]
    [SerializeField] protected float speed = 5f;
    [SerializeField] protected float speedDefault = 5f;
    [SerializeField] protected float speedMax = 30f;
    [SerializeField] protected float speedMin = 0f;
    [SerializeField] protected float speedOffset = 5;
    [SerializeField] protected bool skitter = true;
    [SerializeField] protected float skitterInterval = 2f;
    [SerializeField] protected float skitterTimer;
    [SerializeField] protected float skitterStop = .5f;
    [SerializeField] protected float skitterStopTimer;
    [SerializeField] protected Vector2 velocity = Vector2.zero;
    protected Rigidbody2D body2D;
    protected Vector2 location;

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
    #endregion

    //Runtime Initializations
    private void OnEnable()
    {
        //UpdateOffset();
        health = healthMax * ControllerGame.DifficultySetting();
        speed = speed + Random.Range(0, speedOffset);
        skitterTimer = skitterInterval;
        skitterStopTimer = 0f;
        body2D = GetComponent<Rigidbody2D>();
    }

    //Used to track the number of Seekers in play
    private void OnDestroy()
    {
        ControllerMob.RemoveSeeker();
    }

    //Update is called once per frame
    void Update()
    {
        //Only checks activation if deactive, it will keep trying to chase the player forever.
        if (!isActive)
        {
            isActive = ControllerMob.ActivationCheck(gameObject);
        }
        else
        {
            //Updates movement if Skitter returns true, enabling the bursts of skittering movement.
            UpdateMovement(UpdateSkitter());
        }
        
        
    }

    //When the Seeker collides with another physics object, if that object has the Player tag it injures the target and destroys itself.
    //Uses IVulnerable mechanics
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            IVulnerable target = other.GetComponent("IVulnerable") as IVulnerable;
            target.Injure(damage);
            Kill();
        }
    }

    //Update Functions
    #region

    //UpdateMovment
    //Updates the seekers velocity to aim toward the target. Does not update when very close to the target to reduce the Seekers short range accuracy.
    private void UpdateMovement(bool isMoving)
    {
        if (isMoving)
        {
            Vector2 distanceToTarget = location - targetLocation;
            if (distanceToTarget.sqrMagnitude > 5)
            {
                velocity = SeekingUtilities.CalculateVelocity(gameObject, targetLocation, speed);

            }

            //Deprecated targting logic 
            //Will be removed on next code sweep
            /*if ((location.x - targetLocation.x >= -1) && (location.x - targetLocation.x <= 1))
                {
                    velocity.x *= Mathf.Pow(location.x - targetLocation.x, 2);
                }
                if ((location.y - targetLocation.y >= -1) && (location.y - targetLocation.y <= 1))
                {
                    velocity.y *= Mathf.Pow(location.y - targetLocation.y, 2);
                }*/

            //A lerp is used to smooth turning near the target
            body2D.velocity = Vector2.Lerp(body2D.velocity, velocity, .1f);

            //The sprite is rotated to face the direction of movement.
            body2D.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(body2D.velocity.y, body2D.velocity.x) * Mathf.Rad2Deg - 90, Vector3.forward);
        }
        else
        {
            //If isMoving is false then the velocity is zero.
            body2D.velocity = Vector2.zero;
        }
    }

    //Updates the local variables storing the mobs (known) location and the target's modified location.
    //These are only updated between skitter cycles to reduce accuracy and make it more bug-like.
    private void UpdateLocation()
    {
        location = transform.position;
        targetLocation = SeekingUtilities.Scatter(ControllerPlayer.PlayerLocation, 2, 2);
    }

    //UpdateSkitter
    //Controlls the skittering behavior.
    //Returns a bool allowing it to turn movment on and off easily
    //Utalizes 2 timers, skitterTimer and skitterStopTimer.
    //First the skitterTimer runs, counting down until the skittering is done.
    //When skitterTimer runs out, both skitterTimer and skitterStopTimer are set to their maximum values.
    //First the skitterStopTimer decrements, during this period UpdateSkitter returns false.
    //When skitterStopTimer hits zero (or below) the movment resumes and skitterTimer counts down again.
    private bool UpdateSkitter()
    {
        if (skitterTimer > 0)
        {
            if (skitterStopTimer > 0)
            {
                skitterStopTimer -= Time.deltaTime;
                return false;
            }
            else
            {
                skitterTimer -= Time.deltaTime;
            }
        }
        else
        {
            skitterTimer = skitterInterval;
            skitterStopTimer = skitterStop;
            //UpdateOffset();
            UpdateLocation();
        }

        return true;
    }

    //Deprecated method, will be removed in next code sweep
    /*private void UpdateOffset()
    {
        targetLocationOffset = SeekingUtilities.Scatter(Vector2.zero, 2, 2);
    }*/
    #endregion

    //IVulnerable Functions
    //All use standard IVulnerable/DamageUtilities mechanics
    #region
    public void Heal(float amount)
    {
        DamageUtilities.Heal(this, amount);
    }

    public void Injure(float amount)
    {
        DamageUtilities.Injure(this, amount);
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
    //All use standard IVulnerable/DamageUtilities mechanics
    #region
    public void SpeedDecrease(float amount)
    {
        DamageUtilities.SpeedDecrease(this, amount);
    }

    public void SpeedReset()
    {
        DamageUtilities.SpeedReset(this);
    }

    public void SpeedIncrease(float amount)
    {
        DamageUtilities.SpeedIncrease(this, amount);
    }
    #endregion
}
