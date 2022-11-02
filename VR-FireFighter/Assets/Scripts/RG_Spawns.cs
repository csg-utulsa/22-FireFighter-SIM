using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class RG_Spawns : MonoBehaviour
{
    [Header("Set this to be the right prefab first!!!")]
    public GameObject spawnablePoint;
    [Space(20)]
    [Header("Spawn Locations")]
    [Tooltip("Places that the player can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_player;
    [Tooltip("Places that the fires can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_hazard;
    [Tooltip("Places that the rescueable entities can spawn upon game start. Suggested to use empty game objects w/ RG_Spawnpoint scripts attached for this")]
    public List<RG_Spawnpoint> spawns_rescue;
    [Space(10)]

    [Header("Prefabs and Whatnot")]
    [Tooltip("Stores all the possible 'fire hazard' prefabs that can be spawned. Add additional entries for variety if you want")]
    public GameObject[] pfabs_FireHazard;
    [Tooltip("Stores all the possible 'rescue entity' prefabs that can be spawned. Add additional entries for variety if you want")]
    public GameObject[] pfabs_RescueEntity;
    [Space(10)]

    [Header("Difficulty and Settings")]
    [Tooltip("Number of fire hazards spawned upon game start. Good way to change difficulty level, probably")]
    public int fires = 5;
    [Tooltip("Number of rescue targets spawned upon game start. Good way to change difficulty level, probably")]
    public int rescues = 5;
    // to track the two last used starting points so they aren't reused twice in a row
    List<int> prevSpawns = new List<int>();


    // Start is called before the first frame update
    void Start() {
        SpawnFires();
        SpawnRescues();
        Invoke("SpawnPlayer", 0.1f);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F5)) {
            //GameManager.StartGame();

            SpawnFires();
            SpawnPlayer();
            SpawnRescues();
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
                hazard.transform.parent = GameObject.Find("FireHazards").transform;
                usedSpawns.Add(rand);

                count++;
            }
        }

        // send feedback abt how things went
        if (count < fires) Debug.Log(string.Format("RG_Spawns: !! Not enough spawnpoints to spawn all {0} firehazards requested. Spawned {1} firehazards instead. !!", fires, count));
        else Debug.Log(string.Format("RG_Spawns: Spawned {0} firehazards.", count));
    }

    public void SpawnRescues() {
        // set up some variables
        int count = 0;
        int rand = 0;
        int variant = 0;
        List<int> usedSpawns = new List<int>();

        // loop through and spawn as many rescues as we can
        while (count < rescues && count < spawns_rescue.Count) {
            // get the index of the random spawn position
            rand = Random.Range(0, spawns_rescue.Count);

            // check if this spawn hasn't been used already
            if (!usedSpawns.Contains(rand)) {
                // righty-o, spawn the rescue at the point
                GameObject rescue = Instantiate(pfabs_RescueEntity[variant]);
                rescue.transform.position = spawns_rescue[rand].transform.position;
                rescue.transform.parent = GameObject.Find("RescueTargets").transform;
                usedSpawns.Add(rand);

                count++;
            }
        }

        // send feedback abt how things went
        if (count < rescues) Debug.Log(string.Format("RG_Spawns: !! Not enough spawnpoints to spawn all {0} rescues requested. Spawned {1} rescues instead. !!", rescues, count));
        else Debug.Log(string.Format("RG_Spawns: Spawned {0} rescueEntities.", count));
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
                if (GameObject.Find("PlayerSpawns") != null) sp.gameObject.transform.parent = GameObject.Find("PlayerSpawns").transform;
                break;
                case RG_Spawnpoint.SpawnType.FireHazard:
                if (!spawns_hazard.Contains(sp)) spawns_hazard.Add(sp);
                if (GameObject.Find("HazardSpawns") != null) sp.gameObject.transform.parent = GameObject.Find("HazardSpawns").transform;
                break;
                case RG_Spawnpoint.SpawnType.RescueEnt:
                if (!spawns_rescue.Contains(sp)) spawns_rescue.Add(sp);
                if (GameObject.Find("RescueSpawns") != null) sp.gameObject.transform.parent = GameObject.Find("RescueSpawns").transform;
                break;
            }
        }

        // i don't know why i have to do this, but it places spawnpoint 4 at the start if i don't and yadda yada
        spawns_player.Reverse();
        spawns_hazard.Reverse();
        spawns_rescue.Reverse();

        // check for nulls
        for (int i = 0; i < spawns_player.Count; i++) {
            if (spawns_player[i] == null) spawns_player.RemoveAt(i);
        }
        for (int i = 0; i < spawns_hazard.Count; i++) {
            if (spawns_hazard[i] == null) spawns_hazard.RemoveAt(i);
        }
        for (int i = 0; i < spawns_rescue.Count; i++) {
            if (spawns_rescue[i] == null) spawns_rescue.RemoveAt(i);
        }

        // send debug msg back
        Debug.Log("EDITOR: Sorted RG_Spawns spawnpoints.");
    }

    // call ONLY in editor please
    public void ClearSpawnpoints() {
        spawns_player.Clear();
        spawns_hazard.Clear();
        spawns_rescue.Clear();
    }

    public void CreateNewSpawn(int spawnType) {
        GameObject go = Instantiate(spawnablePoint);
        go.transform.position = transform.position;
        go.transform.parent = transform;
        go.GetComponent<RG_Spawnpoint>().spawnType = (RG_Spawnpoint.SpawnType)spawnType;

        switch (spawnType) {
            case (int)RG_Spawnpoint.SpawnType.Player:
            go.name = "PlayerSpawn";
            break;
            case (int)RG_Spawnpoint.SpawnType.FireHazard:
            go.name = "HazardSpawn";
            break;
            case (int)RG_Spawnpoint.SpawnType.RescueEnt:
            go.name = "RescueSpawn";
            break;
        }

        AutosortSpawnpoints();
    }
}