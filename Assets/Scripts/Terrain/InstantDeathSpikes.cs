using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathSpikes : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        IVulnerable target = collision.gameObject.GetComponent("IVulnerable") as IVulnerable;
        if (target != null)
        {
            target.Kill();
        }
        
    }

}
