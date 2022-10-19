using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RG_Spawns : MonoBehaviour
{
    public List<GameObject> spawns_player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmosSelected() {
        foreach (GameObject go in spawns_player) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(go.transform.position, 0.25f);
        }
    }
}
