using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CONTROLLER: Player
//This is primary controller for the Player GameObject
//It is used for movement, abilities and powers, damage, health and speed.
//Where possible, individual functionality is split off into other functions but where strong interconectivity is useful (such as movement)
//the effects are kept within the PlayerController itself.
//This is built as a *Single Player ONLY* PlayerController. Some functionality could be moved to a Multi-Player build but much cannot.
public class ControllerPlayer : MonoBehaviour, IVulnerable, ISpeedMod {
    //Declatations & Initializations
    #region
    [Space]
    [Header("Health & Damage")]
    [SerializeField] protected float health = 20;
    [SerializeField] protected float healthMax = 20;
    [SerializeField] public float damage = 1;
    [SerializeField] public float damageDefault = 1;
    [SerializeField] private GameObject deathEmission;
    [SerializeField] private bool isInvulnerable = false;
    [SerializeField] protected bool wasInjured = false;
    public static float PlayerDamage = 1;
    //Basic Magic Power
    [SerializeField] protected GameObject magicBullet;
    //Slow Magic Power
    [SerializeField] protected GameObject slowBullet;
    [SerializeField] protected float rateOfFire = 10;
    protected float shootTimer;


    //IVulnerable Properties
    #region
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
    #endregion

    [SerializeField] protected bool isDead = false;
    [Space]
    [Header("Special Powers")]

    //Levitate Power
    [SerializeField] protected bool powerLevitate = false;
    [SerializeField] protected bool levitateIsLevitating = false;
    [SerializeField] protected GameObject levitationEffect;
    //Multi-Jump Power
    [SerializeField] protected bool powerMultiJump = false;
    [SerializeField] protected float multiJumpCount;
    [SerializeField] protected float multiJumpCountMax = 2f;
    //Jump Jet Power
    [SerializeField] protected bool powerJumpJets = false;
    [SerializeField] protected float jumpJetTimer;
    [SerializeField] protected float jumpJetTimerMax = .4f;
    

    [Space]
    [Header("Physics & Movement")]
    [SerializeField] protected Transform startLocation;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float speedDefault = 10f;
    [SerializeField] protected float speedMax = 30f;
    [SerializeField] protected float speedMin = 0f;

    //ISpeedMod Properties
    #region
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

    [SerializeField][Range(.01f, 1f)] protected float accelerationGround = .5f;
    [SerializeField] [Range(0f, 1f)] protected float accelerationAir = .1f;
    [SerializeField] protected float jumpSpeed = 15f;
    protected bool isJumping = false;
    
    [SerializeField] protected Vector2 velocity;
    [SerializeField] protected bool onGround;
    [SerializeField] protected float maxAccelerationGround = 10f;

    
    [Space]
    [Header("Objects")]
    [SerializeField] protected Transform firePoint;
    protected Rigidbody2D body2D;
    
    protected Collider2D[] colliders;
    protected SpriteRenderer sprite;
    private CapsuleCollider2D capsule;

    [Space]
    [Header("Testing Fields")]
    [SerializeField] public Vector2 pcLocTest;

    //Statics
    //pcLoc is used by various targetting system to make locating the player efficient.
    //This method is only single-player friendly
    public static Vector2 PlayerLocation = Vector2.zero;
    public static float PlayerHealth { get; set; }



    #endregion

    //OnEnable contains all of the initializations for PlayerController.
    //Any initializations that are reset upon death are in InitializeValues();
    //Any initializations that are OnEnable only, are in the function itself.
  

    void OnEnable () {
        body2D = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        capsule = GetComponent<CapsuleCollider2D>();
        InitalizeValues();
    }
	
	//Update functionality is broken up into smaller methods
    //See specific methods.
	void Update () {
        //Checks if the player is/should be dead
        UpdateDeathCheck();

        //Player update functions only run if the game is not paused
        if (ControllerGame.IsPlay)
        {
            //Syncronizing Velocity
            velocity = body2D.velocity;

            
            
            //Checks isGrounded status
            UpdateGroundCheck();

            //Handles jump acceleration and timers
            UpdateJump();

            //Handles X velocity change
            UpdateMove();

            //Handles shooting
            UpdateShoot();

            //Updates the Unity Physics2D values with the local values
            UpdateVelocity();

        }
    }

    //InitializeValues
    //Sets values to their starting defaults
    //Called during Start and DeathReset.
    private void InitalizeValues()
    {
        health = healthMax;
        damage = damageDefault;
        onGround = false;
        transform.position = startLocation.position;
        PlayerLocation = body2D.position;
        PlayerHealth = health;
    }

    //DeathReset
    //Resets the player after death.
    //Currently resets all values to default starting values.
    public void DeathReset()
    {
        InitalizeValues();
    }

    //Update Functions
    #region
    //UpdateDeathCheck
    //Checks to see if the player should be dead.
    private void UpdateDeathCheck()
    {
        PlayerHealth = health;

        if ((health <= 0) && (isInvulnerable != true))
        {
            isDead = true;
            
        }

        if ((isDead) && (ControllerGame.IsPlayerDead != true))
        {
            ControllerGame.PlayerDead(isDead);
            sprite.enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<PolygonCollider2D>().enabled = false;
            if (deathEmission != null)
            {
                Instantiate(deathEmission, transform.position, transform.rotation);
            }
        }

        if (isDead)
        {
            body2D.velocity = Vector2.zero;
        }

    }

    //GroundCheck
    //If the capsul collider is colliding with ground then isGrounded becomes true.
    private void UpdateGroundCheck()
    {
        if ((capsule.IsTouchingLayers(LayerMask.GetMask("Ground"))) || (capsule.IsTouchingLayers(LayerMask.GetMask("Mob"))) || capsule.IsTouchingLayers(LayerMask.GetMask("Ghost")))
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

    }

    
    //UpdateJump
    //The Jumping mechanics including the Multi-Jump, Jump-Jet and Levitate powers.
    //The basic mechanic is that on the first update after the player presses jump or up on the Y axis the character will jump if they are on the ground.
    //Specific power mechanics are described within the method.
    private void UpdateJump()
    {
        /*Jump Code
        *If the player presses a Jump input (the jump key or up on a Y-axis imput) Then the code will check to see if they qualify for a jump.
        *Jumps will only trigger on the KeyDown (to avoid jumping in successive frames).
        *The order of the powers is specific, resulting in an intended multi-power interaction (jump jet cooldown is used before multi-jump charges, for instance).
        *The Levitation code works with the jump system and so is included below.*/

        //isJumping is used to see if anything has triggered a jump this frame. If a jump has been triggered no additional jumps will trigger.
        isJumping = false;

        //STANDARD JUMP
        if((onGround == true) && (ControllerInput.JumpClick == true)){
            velocity.y = jumpSpeed;
            isJumping = true;
        }

        /*JUMP JETS
        *Jumps in the air after a fixed cooldown.
        *A short cooldown allows for a clumsy sort of flight, a long cooldown may prove useless (if the time to the ground is longer than the cooldown).*/
        if (powerJumpJets == true)
        {
            //Jump Jet
            if ((ControllerInput.JumpClick == true) && (isJumping == false) && (jumpJetTimer <= 0))
            {
                velocity.y = jumpSpeed;
                isJumping = true;
                jumpJetTimer = jumpJetTimerMax;
            }
            //Jump Jet Recharge
            if (jumpJetTimer > 0)
            {
                jumpJetTimer -= Time.deltaTime;
            }
        }

        /* MULTI-JUMP
         * Allows a fixxed number of jumps after leaving the ground.
         * Unlike Jump Jets, Multi-Jump only recharges on the ground.
        */
        if (powerMultiJump == true)
        {
            //Multi-Jump
            if ((ControllerInput.JumpClick == true) && (isJumping == false) && (multiJumpCount > 0))
            {
                velocity.y = jumpSpeed;
                isJumping = true;
                multiJumpCount--;
            }

            //Multi-Jump Recharge
            if (onGround == true)
            {
                multiJumpCount = multiJumpCountMax;
            }
        }

        /*LEVITATION
         * Freezes the players Y axis while the Jump button is held. Executes last so it will only trigger if the player has no jumps available.
         * X input is deactivated while the player is levitating.
         */
        if (powerLevitate == true)
        {
            if ((isJumping == false) && (onGround == false) && (ControllerInput.JumpClick == true))
            {
                levitateIsLevitating = true;
            }
            else if (ControllerInput.JumpHold == false)
            {
                levitateIsLevitating = false;
            }

            if (levitateIsLevitating == true)
            {
                ControllerInput.X = 0;
                body2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                levitationEffect.SetActive(true);
            }
            else
            {
                body2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                levitationEffect.SetActive(false);
            }

        }
    }

    //UpdateMove
    //As long as the player is on the ground or pressing an X input their X velocity will lerp towards the direction of the input 
    //(or zero if on the ground with no imput) from their current velocity.
    //If in the air with no x input they will continue at their current rate.
    private void UpdateMove()
    {
        if (onGround == true)
        {
            velocity.x = Mathf.Lerp(body2D.velocity.x, ControllerInput.X * Speed, accelerationGround);
        }
        else if (ControllerInput.X != 0)
        {
            velocity.x = Mathf.Lerp(body2D.velocity.x, ControllerInput.X * Speed, accelerationAir);
        }
        
    }


    //UpdateShoot
    //The player has a left button fire and a right button fire
    //If holding the button they will shoot rateOfFire shots per second divided by the attacks speed.
    private void UpdateShoot()
    {
        //Updates the global player damage variable
        PlayerDamage = damage;

        //Basic Missile
        //Speed 1
        if ((ControllerInput.MouseLeftHold) && (shootTimer <=0))
        {
            if (shootTimer <= 0)
            {
                Instantiate(magicBullet, firePoint.position, firePoint.rotation);
                shootTimer = 1/rateOfFire;
            }
        }
        //Slow Missile
        //Speed 5
        else if ((ControllerInput.MouseRightHold) && (shootTimer <= 0))
        {
            if (shootTimer <= 0)
            {
                Instantiate(slowBullet, firePoint.position, firePoint.rotation);
                shootTimer = 5 / rateOfFire;
            }
        }
        //Timer countdown.
        else if (shootTimer >0)
        {
            shootTimer -= Time.deltaTime;
        }
    }

    //UpdateVelocity
    //Updates the Player's Unity Physics velocity and checks if the sprite needs to be flipped.
    //TODO: Move sprite flip to state machine when/if more character sprites are added.
    //Updates the static PlayerLocation variable with the current location.
    private void UpdateVelocity()
    {
        //Sets UnityPhysics velocity based on class velocity and then checks for a sprite direction change as long as velocity is not zero
        body2D.velocity = velocity;
        if (ControllerInput.X < 0)
        {
            sprite.flipX = true;
        }
        else if (ControllerInput.X > 0)
        {
            sprite.flipX = false;
        }

        //Update the PC's static location.
        PlayerLocation = body2D.position;
        //pcLocTest = pcLoc;
    }

    #endregion

    //ISpeedMod Functions
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

    //IVulnerable functions
    #region
    public void Heal(float amount)
    {
        DamageUtilities.Heal(this, amount);
    }

    public void Injure(float amount)
    {
        if ((!isInvulnerable) && (!wasInjured))
        {
            DamageUtilities.Injure(this, amount);
            wasInjured = true;
            sprite.color = Color.red;
            StartCoroutine(InjuryReset());
        }
    }

    public void Kill()
    {
        if (!isInvulnerable){ 
            isDead = true;
            health = 0;
        }
    }

    IEnumerator InjuryReset()
    {
        yield return new WaitForSeconds(.5f);
        wasInjured = false;
        sprite.color = Color.white;
    }
    #endregion
}
