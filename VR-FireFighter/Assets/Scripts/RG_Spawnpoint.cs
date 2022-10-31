using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_Spawnpoint : MonoBehaviour
{
    // setup vars
    [Tooltip("Type of object that this point can spawn")]
    public SpawnType spawnType;

    [Tooltip("Enum containing the different types of spawnpoints.")]
    public enum SpawnType
    {
        Player,
        FireHazard,
        RescueEnt,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        switch (spawnType) {
            case SpawnType.Player:
                Gizmos.color = Color.blue;
            break;
            case SpawnType.FireHazard:
                Gizmos.color = Color.red;
            break;
            case SpawnType.RescueEnt:
                Gizmos.color = Color.yellow;
            break;
        }
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
