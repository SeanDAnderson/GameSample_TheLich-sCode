using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTBugSpawn : MonoBehaviour {

    // For creating a Mob Spawner in testing. NOT a normal game spawner (does not check for spawn caps).
    //Only usable in a debug enabled build, not in a live build

#if DEBUG
    //Declarations & Initializations
    #region
    [SerializeField] GameObject prefab;
    [SerializeField] private float spawnTime = .5f;
    private float timer;
    #endregion

    void Start () {
        timer = 0;
	}
	
	//Spawns a clone of the prefab every 'timer' seconds
	void Update () {
        timer += Time.deltaTime;
        if (timer >= spawnTime) 
        {
            Instantiate(prefab, gameObject.transform.position, gameObject.transform.rotation);
            timer = 0;
        }
    }
#endif
}
