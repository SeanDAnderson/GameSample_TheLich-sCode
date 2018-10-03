using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//CONTROLLER: Missile
//Controller for projectile objects.
//Can be used by player or mob missiles.
//Assigns a velocity, hit conditions and hit effects.
public class ControllerMissile : MonoBehaviour {

    //Declarations & Initializations
    #region
    //Enumorators
    public enum EffectType {None, Damage, Slow};
    public enum TargetType {None, MouseCursor, Player};

    [Header("Damage & Effect")]
    [SerializeField] protected TargetType targetType = 0;
    protected Vector2 targetLocation = Vector2.zero;
    [SerializeField] protected string hitTag = "Bug";
    [SerializeField] protected EffectType effect = 0;
    [SerializeField] protected bool usePlayerDamage = false;
    [SerializeField] protected float effectAmount = 1;
    [SerializeField] protected bool destroyedOnImpact = true;
    [SerializeField] protected GameObject deathEmission;
    
    [Space]
    [Header("Physics & Movement")]
    [SerializeField] protected float speed = 20f;
    [SerializeField] protected float missileLifespan = 5f;
    [SerializeField] protected Vector2 missileVelocity = Vector2.zero;
    #endregion


    //Runtime Initializations
    //Largely based on specific missile conditionals.
    private void OnEnable()
    {
        //Getting the location of the targety point.
        //Note, this is not a seeking missile, it merely fires toward the target point, it does not update the target after firing.
        GetTargetLocation();

        //Setting velocity
        GetComponent<Rigidbody2D>().velocity = SeekingUtilities.CalculateVelocity(gameObject, targetLocation, speed);

        //Setting destruction after missileLifespan seconds
        Destroy(gameObject, missileLifespan);

        //Setting damage to player damage if usePlayerDamage is true
        if (usePlayerDamage == true)
        {
            effectAmount = ControllerPlayer.PlayerDamage / ControllerGame.DifficultySetting();
        }

    }

    //OnTriggerEnter2D
    //Handles collision event, checking for a valid collision
    //Only interacts with specified target type. Used IVulnerable and ISpeedMod for effects.
    //Effect depends on chosen Effect enum.
    //Will destroy the missile after impact if destroyOnImpact is true.
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == hitTag)
        {
            //Applies effect based on selected effect type.
            //Note: Incompatable effects (hitting a non-ISpeedMod target with a slow) will still trigger here, but will fail.
            switch (effect)
            {
                case EffectType.None:
                    Debug.Log("Hit " + other);
                    break;
                case EffectType.Damage:
                    IVulnerable damageTarget = other.GetComponent("IVulnerable") as IVulnerable;
                    damageTarget.Injure(effectAmount);
                    break;
                case EffectType.Slow:
                    ISpeedMod speedTarget = other.GetComponent("ISpeedMod") as ISpeedMod;
                    speedTarget.SpeedDecrease(effectAmount);
                    break;
                default:
                    Debug.Log("Effeect not found " + effect);
                    break;
            }

            //Destroys the target if set to destroyOnImpact. Otherwise projectile will destroy after missileLifespan.
            if(destroyedOnImpact == true)
            {
                Destroy(gameObject);
                if (deathEmission != null)
                {
                    Instantiate(deathEmission, transform.position, transform.rotation);
                }
            }
        }
    }

    //GetTargetLocation
    //Gets the location of the specified target type.
    //If type 'None' then a random point withing 10 unitys (square) is selected.
    private void GetTargetLocation()
    {
        switch (targetType)
        {
            case TargetType.None:
                targetLocation = new Vector2(Random.Range(-10,10), Random.Range(10,10));
                break;
            case TargetType.MouseCursor:
                targetLocation = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
                break;
            case TargetType.Player:
                targetLocation = ControllerPlayer.PlayerLocation;
                break;
            default:
                Debug.Log("Unknown target type " + targetType);
                break;

        }
    }

}
