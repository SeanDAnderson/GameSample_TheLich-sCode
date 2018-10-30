using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The IVulnerable interface is used to allow generic health, damage, and destruction interactions on mobs.
//Any mob with the interface can be affected by the required methods, other mobs are ignored.
//All generic implimentations are described here, bespoke implementations are detailed in the mob's Controller class.
public interface IVulnerable{

    //General Healing Mechanic
    //Normal implementation handled in DamageUtilities
    //                  Properties
    //float health      Used for storing current creature health. Get/Set
    //float healthMax   Used for setting creatures maximum health. maxHealth of 0 means no health limit. Negative values will result in any injury destroying the target. Get/Set
    //float damage      How much health the target loses when the creature injures it. Get/Set

    float Health { get; set; }
    float HealthMax { get; set; }
    float Damage { get; set; }

    //                  Functions
    //      Heal()      Adjusts the targets health, checking to ensuure that it does not exceed the maximum health. A zero value will simply perform a 'max health' check.
    //                  A negative value will reduce the targets health but not destroy it (no zero health check is performed).
    //float amount      The amount to be added to health. A positive, negative or zero value is acceptable.
    //
    //      Injure()    Adjuste the targets health, lowering it by the input amount and destroying targets with zero or fewer health. A zero value will perform a 'destuction check' 
    //                  destroying a target with 0 or fewer health. A negative value will add to the targets health, allowing the health to be raised above it's healthMax. 
    //float amount      The amount to be subtracted from the targets health. A positive, negative or zero value is acceptable.
    //
    //      Kill()      Destroys the game object or initiates another death state.

    void Heal(float amount);
    void Injure(float amount);
    void Kill();

    


    

}
