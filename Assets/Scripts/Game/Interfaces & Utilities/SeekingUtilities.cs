using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//SEEKING UTILITIES
//Tools for any target seeking mobile mob.
public static class SeekingUtilities {
    
    //CalculateVelocity
    //Calculates a Vector2 velocity to move from the mobs location to the target location at speed.
    public static Vector2 CalculateVelocity(GameObject mob, Vector2 target, float speed)
    {
        //Subtracts the spawnpoint postion from the mouse poisition to get the relative location of the mouse from the spawnpoint
        Vector2 relativeLoc = new Vector2((target.x - mob.transform.position.x), (target.y - mob.transform.position.y));
        
        //Gets the magnitude of the relative location
        float relativeMagnitude = relativeLoc.magnitude;
        
        //Sets the x and y velocities based on the relative magnitude (so that the projectile moves at the full missileSpeed toward the cursor)
        return new Vector2((relativeLoc.x / relativeMagnitude) * speed, (relativeLoc.y / relativeMagnitude) * speed);
    }

    //Scatter
    //Returns a Vector scattered in X and Y within a given range with a granularity of .01 units
    //Vector2   target          The initial, unscattered target location
    //Vector2   scatterX        The maximum scatter on the X axis.
    //Vector2   scatterY        The maximum scatter on the Y axis.
    public static Vector2 Scatter(Vector2 target, float scatterX, float scatterY)
    {
        //Setting target X to a value between -scatterX and +scatterX with a granularity of .01
        //Random only produces integers so values are multiplied and devided to achieve a float with the desired granularity.
        target.x = target.x + Random.Range(-scatterX * 100, scatterX * 100) / 100;
        //Setting target Y as above
        target.y = target.y + Random.Range(-scatterY * 100, scatterY * 100) / 100;

        //Returns the scattered target location
        return target;
    }
}
