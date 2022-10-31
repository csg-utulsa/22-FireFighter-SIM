using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RG_Spawns : MonoBehaviour
{
    [Header("Spawn Locations")]
    [Tooltip("Places that the player can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_player;
    [Tooltip("Places that the fires can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_hazard;
    [Tooltip("Places that the rescueable entities can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_rescue;
    [Space(5)]

    [Header("Prefabs and Whatnot")]
    [Tooltip("Stores all the possible 'fire hazard' prefabs that can be spawned. Add additional entries for variety if you want")]
    public GameObject[] pfabs_FireHazard;
    [Tooltip("Stores all the possible 'rescue entity' prefabs that can be spawned. Add additional entries for variety if you want")]
    public GameObject[] pfabs_RescueEntity;
    [Space(5)]

    [Header("Difficulty and Settings")]
    [Tooltip("Number of fire hazards spawned upon game start. Good way to change difficulty level, probably")]
    public int fires = 5;
    // to track the two last used starting points so they aren't reused twice in a row
    List<int> prevSpawns = new List<int>();


    // Start is called before the first frame update
    void Start() {
        SpawnFires();
        Invoke("SpawnPlayer", 0.1f);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            SpawnFires();
            SpawnPlayer();
        }
    }

    // spawn player
    public void SpawnPlayer() {
        // get the index of the random spawn position
        int rand = Random.Range(0, spawns_player.Count);
        // make sure it doesn't explode itself
        if (prevSpawns.Count > 0) {
            // only reroll if current spawn matches one of the two previously used spawns
            while ((rand == prevSpawns[0] || (prevSpawns.Count > 1 && rand == prevSpawns[1])) && !(Random.Range(0, 10) > 9)) {
                rand = Random.Range(0, spawns_player.Count);
            }
        }
        // store last spawn
        prevSpawns.Add(rand);
        // remove oldest spawn (if size capacity reached)
        if (prevSpawns.Count >= 2) prevSpawns.RemoveAt(0);

        // move the player to the point
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = spawns_player[rand].transform.position;
        player.GetComponent<CharacterController>().enabled = true;

        // output
        Debug.Log("spawned at: " + rand);
    }


    public void SpawnFires() {
        // set up some variables
        int count = 0;
        int rand = 0;
        int variant = 0;
        List<int> usedSpawns = new List<int>();
        
        // loop through and spawn as many hazards as we can
        while (count < fires && count < spawns_hazard.Count) {
            // get the index of the random spawn position
            rand = Random.Range(0, spawns_hazard.Count);

            // check if this spawn hasn't been used already
            if (!usedSpawns.Contains(rand)) {
                // righty-o, spawn the hazard at the point
                GameObject hazard = Instantiate(pfabs_FireHazard[variant]);
                hazard.transform.position = spawns_hazard[rand].transform.position;
                usedSpawns.Add(rand);

                count++;
            }
        }

        // send feedback abt how things went
        if (count < fires) Debug.Log(string.Format("RG_Spawns: !! Not enough spawnpoints to spawn all {0} firehazards requested. Spawned {1} firehazards instead. !!", fires, count));
        else Debug.Log(string.Format("RG_Spawns: Spawned {0} firehazards.", count));
    }

    private void OnDrawGizmosSelected() {
        foreach (RG_Spawnpoint sp in spawns_player) {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(sp.transform.position, 0.25f);
        }
        foreach (RG_Spawnpoint sp in spawns_hazard) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(sp.transform.position, 0.25f);
        }
    }

    // call this ONLY in the editor. can't make any promises it won't explode if you do otherwise
    public void AutosortSpawnpoints() {
        // put the spawnpoint in its proper list
        RG_Spawnpoint[] sps = Object.FindObjectsOfType<RG_Spawnpoint>();

        // okay, lets get this over with
        foreach (RG_Spawnpoint sp in sps) {
            switch (sp.spawnType) {
                case RG_Spawnpoint.SpawnType.Player:
                    if (!spawns_player.Contains(sp)) spawns_player.Add(sp);
                break;
                case RG_Spawnpoint.SpawnType.FireHazard:
                    if (!spawns_hazard.Contains(sp)) spawns_hazard.Add(sp);
                break;
                case RG_Spawnpoint.SpawnType.RescueEnt:
                    if (!spawns_rescue.Contains(sp)) spawns_rescue.Add(sp);
                break;
            }
        }

        // i don't know why i have to do this, but it places spawnpoint 4 at the start if i don't and yadda yada
        spawns_player.Reverse();
        spawns_hazard.Reverse();
        spawns_rescue.Reverse();

        // send debug msg back
        Debug.Log("EDITOR: Sorted RG_Spawns spawnpoints.");
    }

    // call ONLY in editor please
    public void ClearSpawnpoints() {
        spawns_player.Clear();
        spawns_hazard.Clear();
        spawns_rescue.Clear();
    }
}