﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//MOBCONTROLLER
//Used to handle global Bug mechanics (maximum spawning counts) and can be expanded to handle common mob mechanics.
//Currently a glorified bug counter.
public class ControllerMob : MonoBehaviour {

    //Declarations & Initializations
    #region
    //TODO More bugs!
    public enum BugTypes {Seeker, Spawner};

    //Activation
    [SerializeField]protected float activationRange = 15;
    public static float ActivationRange { get; set; }

    //Total Bug Counting
    [SerializeField]protected float maxBugs = 50;
    public static float CurrentBugs = 0;

    public static float MaxBugs { get; set; }

    //Seeker Counting
    [SerializeField]protected float maxSeekers = 50;
    public static float CurrentSeekers = 0;
    public static float MaxSeekers { get; set; }

    //Spawners are not intended to be spawned automatically so there is no counting mechanism for them
    #endregion

    //Runtime assignments
    private void Awake()
    {
        ActivationRange = activationRange;
        MaxBugs = maxBugs;
        MaxSeekers = maxSeekers;
    }

    //Attempts to add a new Seeker bug if the cap has not been reached
    //GameObject    spawn       The prefab game object to be spawned (The Seeker object)
    //GameObject    spawner     The object calling the method, attempting to spawn the Seeker.
    //RETURNS
    //bool                      Used to inform the spawning object if the spawn attempt was successful.
    public static bool AddSeeker(GameObject spawn, GameObject spawner, float spawnCount)
    {
        if (spawnCount >= 0)
        {
            Instantiate(spawn, spawner.transform.position, spawner.transform.rotation);
            CurrentBugs++;
            CurrentSeekers++;
            return true;
        }
        else if ((CurrentSeekers < MaxSeekers) && (CurrentBugs < MaxBugs))
        {
            Instantiate(spawn, spawner.transform.position, spawner.transform.rotation);
            CurrentBugs++;
            CurrentSeekers++;
            return true;
        }

        Debug.Log("Too many bugs");
        return false;
    }
    
    //Removes a Seeker from the count.
    //Called in OnDestroy in the Seeker class.
    public static void RemoveSeeker()
    {
        CurrentBugs--;
        CurrentSeekers--;
    }

    //Checks if the object is within activation range of the player.
    //Used to prevent costly caluclations and spawning by mobs too far away from the player to contribute to gameplay (i.e. off the screen in most cases). 
    public static bool ActivationCheck(GameObject bug)
    {
        //Checks the distance on both the X and Y axes. Only activates if the player is withing range on both.
        if ((ControllerPlayer.PlayerLocation.x > (bug.transform.position.x - ActivationRange)) && (ControllerPlayer.PlayerLocation.x < (bug.transform.position.x + ActivationRange)) &&
            (ControllerPlayer.PlayerLocation.y > (bug.transform.position.y - ActivationRange)) && (ControllerPlayer.PlayerLocation.y < (bug.transform.position.y + ActivationRange)))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
