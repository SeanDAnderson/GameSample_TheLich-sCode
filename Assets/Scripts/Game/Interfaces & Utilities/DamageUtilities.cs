using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DAMAGE UTILITIES
//Provides most common functionality for the IVulnerable and ISpeedMod interface methods.
//See the interface classes for more details on these generic implimentations. 
public static class DamageUtilities {

    //IVulnerable Functions
    //See IVulnerable.cs
    public static void Injure(IVulnerable target, float amount)
    {
        target.Health -= amount;
        if (target.Health <= 0)
        {
            target.Kill();
        }
    }

    public static void Heal(IVulnerable target, float amount)
    {
        target.Health += amount;
        if (target.Health < target.HealthMax)
        {
            target.Health = target.HealthMax;
        }
    }

    //ISpeedMod Functions
    //See ISpeedMod.cs

    public static void SpeedDecrease(ISpeedMod target, float amount)
    {
        target.Speed -= amount;
        if (target.Speed < target.SpeedMin)
        {
            target.Speed = target.SpeedMin;
        }
    }

    public static void SpeedReset(ISpeedMod target)
    {
        target.Speed = target.SpeedDefault;
    }

    public static void SpeedIncrease(ISpeedMod target, float amount)
    {
        target.Speed += amount;
        if (target.Speed > target.SpeedMax)
        {
            target.Speed = target.SpeedMax;
        }
    }

}
