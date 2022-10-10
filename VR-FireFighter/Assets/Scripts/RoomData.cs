using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class RoomData : MonoBehaviour
{
    public class Exits {
        public GameObject exit_east;
        public GameObject exit_west;
        public GameObject exit_north;
        public GameObject exit_south;

        public bool has_exit_east = false;
        public bool has_exit_west = false;
        public bool has_exit_north = false;
        public bool has_exit_south = false;

        public Exits() {

        }

        public Exits(GameObject exit_east, GameObject exit_west, GameObject exit_north, GameObject exit_south) { 
            this.exit_east = exit_east;
            this.exit_west = exit_west;
            this.exit_north = exit_north;
            this.exit_south = exit_south;
        }

        public void BlockAllExits() {
            exit_east.SetActive(true);
            exit_west.SetActive(true);
            exit_north.SetActive(true);
            exit_south.SetActive(true);
        }
    }

    public Exits exits;
    public Transform doorBlocks;
    public Vector2 gridPosition;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup() {
        exits = new Exits();

        exits.exit_east = doorBlocks.GetChild(0).gameObject;
        exits.exit_west = doorBlocks.GetChild(1).gameObject;
        exits.exit_north = doorBlocks.GetChild(2).gameObject;
        exits.exit_south = doorBlocks.GetChild(3).gameObject;

        Debug.Log(name + "Created exits");

        exits.BlockAllExits();
    }

    public void OpenExits() {
        if (exits.has_exit_east) exits.exit_east.SetActive(false);
        if (exits.has_exit_west) exits.exit_west.SetActive(false);
        if (exits.has_exit_north) exits.exit_north.SetActive(false);
        if (exits.has_exit_south) exits.exit_south.SetActive(false);

        Debug.Log(name + "opened exits!");
    }
}
