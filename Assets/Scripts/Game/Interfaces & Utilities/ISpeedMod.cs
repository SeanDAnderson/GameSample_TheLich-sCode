using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpeedMod{

    //General Speed Modificaton Mechanic
    //These are the default uses and mechanics implemented in DamageUtilities.
    //Individual ISpeedMod classes may have different implementations.
    //Allows manipulation of a targets Speed, typically tied to some ability that can be performed faster or slower including movement, spawning and/or attacking.
    //              Properties
    //float         Speed           The target's current Speed
    //float         SpeedMax        The target's maximum Speed, used to cap modifications to Speed.
    //Float         SpeedMin        The target's minum Speed, used to prevent Speed from falling below a set floor. Defaults to 0 in most objects.
    //Float         SpeedDefault    The target's starting Speed, typically used to reset the target to their starting speed. 

    float Speed { get; set; }
    float SpeedMax { get; set; }
    float SpeedMin { get; set; }
    float SpeedDefault { get; set; }


    //              Methods
    //          SpeedDecreats       Used to decrease the target's Speed by amount. If the decrease drops Speed to less than SpeedMin, it is set to SpeedMin.
    //float         amount          The amount speed is decreased by.
    //
    //          SpeedIncrease       Used to increast the target's Speed by amount. If the increase makes Speed greater than SpeedMax, it is set to SpeedMax.
    //float         amount          The amount speed is increased by.
    //
    //          SpeedReset          Used to set the target's speed equal to SpeedDefault.

    void SpeedDecrease( float amount);
    void SpeedIncrease( float amount);
    void SpeedReset();
    
}
