using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTBugSpawn : MonoBehaviour {

    // For creating a Mob Spawner in testing. NOT a normal game spawner (does not check for spawn caps).

#if DEBUG
    [SerializeField] GameObject prefab;
    [SerializeField] private float spawnTime = .5f;
    private float timer;



	
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
