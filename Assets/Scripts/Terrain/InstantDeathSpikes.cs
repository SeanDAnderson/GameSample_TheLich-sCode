using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The script for making spikes instantly kill mobs. Kills both player and AI mobs.
public class InstantDeathSpikes : MonoBehaviour {

    //When the spikes detect a collision, it attempts to exectute the target's Kill method.
    //If the target is not an IVulnerable object, the script does nothing.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        IVulnerable target = collision.gameObject.GetComponent("IVulnerable") as IVulnerable;
        if (target != null)
        {
            target.Kill();
        }
        
    }

}
