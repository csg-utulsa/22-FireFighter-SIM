using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


/*public class LayoutBounds : MonoBehaviour {
    Vector3 top;
    Vector3 bottom;
    public LayoutBounds(Vector3 _top, Vector3 _bottom) {
        top = _top;
        bottom = _bottom;
    }
}*/

public class LayoutRandomizer : MonoBehaviour
{
    // vars
    public GameObject[] room_hallways;
    public GameObject[] room_main;
    public GameObject[] room_side;
    public GameObject[] room_exit;
    public Vector3 layoutBounds;
    public Vector2 cells;

    // Start is called before the first frame update
    void Start()
    {
        layoutBounds = new Vector3( layoutBounds.x * transform.localScale.x, layoutBounds.y * transform.localScale.y, layoutBounds.z * transform.localScale.z);
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5)) ResetMaze();
    }

    void Generate() {
        GameObject[,] rooms         = new GameObject[(int)cells.x, (int)cells.y];
        List<GameObject> rooms_sealed = new List<GameObject>();
        //[(int) cells.x][(int) cells.y]

        // create basic rooms
        Vector3 startpos = transform.position - new Vector3(layoutBounds.x / 2, 0, layoutBounds.z / 2);
        for (var r = 0; r < cells.x; r++) {
            for (var c = 0; c < cells.y; c++) {
                GameObject go = Instantiate( room_main[Mathf.RoundToInt(Random.Range(0,2))], transform );
                go.name = "Room (" + r + ", " + c + ")";
                go.transform.position = startpos + new Vector3(c * (layoutBounds.x / cells.x), 0, r * (layoutBounds.z / cells.y));

                rooms[r,c] = go;
            }
        }

        // pick a random starting point along the edge of the maze
        Vector2 cells_ad = new Vector2(cells.x - 1, cells.y - 1);
        Vector2 startPos = new Vector2( Mathf.Round(Random.Range(0f, 1f)) * cells_ad.x, Mathf.Round(Random.Range(0f, 1f)) * cells_ad.y );
        Vector2 endPos = new Vector2(-1, -1);
        GameObject entrance;
        GameObject unentrance = null;
        float expectedProximity = Mathf.Max(cells.x - 1f, cells.y - 1f);
        int attempts = 0;
        while ( (startPos == endPos || endPos == (Vector2.one * -1.0f) ) && attempts < 25) {
            // generate an end position

            // determine side to place the exit on
            int sideSeed = Mathf.RoundToInt( Random.Range(0f, 3f) );
            if (sideSeed > 0 && sideSeed < 3) {
                endPos = new Vector2( sideSeed - 1, Mathf.Round(Random.Range(0f, 1f)) * cells_ad.y );
            } else {
                endPos = new Vector2( Mathf.Round(Random.Range(0f, 1f)) * cells_ad.x, Mathf.Round(sideSeed / 3) );
            }

            // check for proximity
            if (!(Vector2.Distance(startPos, endPos) >= expectedProximity) || (startPos.x == endPos.x || startPos.y == endPos.y) ) {
                Debug.Log("startPos: "+startPos+" | "+endPos+" | proximity: "+Vector2.Distance(startPos, endPos));

                endPos = new Vector2(-1, -1);
            } else {
                Debug.Log("found a good exit! startPos: " + startPos + " | " + endPos + " | proximity: " + Vector2.Distance(startPos, endPos));

                // get entrance
                GameObject oldEnt   = rooms[(int)startPos.x, (int)startPos.y];
                Vector3 oldPos      = oldEnt.transform.position;
                entrance            = Instantiate(room_exit[0], transform);

                entrance.transform.position = oldPos;
                entrance.name = "Enter " + oldEnt.name;
                entrance.transform.SetSiblingIndex(oldEnt.transform.GetSiblingIndex());

                rooms[(int)startPos.x, (int)startPos.y] = entrance;
                Destroy(oldEnt);


                // calling it unentrance b/c exit is a reserved keyword
                oldEnt                  = rooms[(int)endPos.x, (int)endPos.y];
                oldPos                  = oldEnt.transform.position;
                unentrance              = Instantiate(room_exit[1], transform);

                unentrance.transform.position = oldPos;
                unentrance.name = "Exit " + oldEnt.name;
                unentrance.transform.SetSiblingIndex(oldEnt.transform.GetSiblingIndex());

                rooms[(int)endPos.x, (int)endPos.y] = unentrance;
                Destroy(oldEnt);
            }

            attempts++;
        }

        // now go thru and try to add doors
        Vector2 testPos = new Vector2(startPos.x, startPos.y);
        Vector2 offset  = Vector2.zero;
        List<GameObject> rooms_visited = new List<GameObject>();
        List<GameObject> halls = new List<GameObject> ();
        bool exitfound = false;
        attempts = 0;

        // loop thru, if find an exit, we're done, otherwise keep trying
        while (!exitfound && attempts < 20) {
            GenerateStep(testPos, rooms_visited, halls, rooms, unentrance);

            if (rooms_visited.Contains(unentrance)) {
                exitfound = true;
            } else {
                rooms_visited = new List<GameObject>();
                for (var i = 0; i < halls.Count; i++) {
                    Destroy(halls[i]);
                }
                halls = new List<GameObject>();

                attempts++;
            }
        }
        // if we still can't find an exit, start from scratch
        if (!exitfound) {
            Debug.Log("Could not find an exit");
            ResetMaze();
        }

        // okay, we have our grid, an entrance/exit, and a correct path to the goal. time to start some trickery
        int maxFakeDoors        = Mathf.RoundToInt(cells.x * cells.y) - halls.Count;
        int fakeDoors           = 0;
        List<GameObject> fakes  = new List<GameObject>();
        Debug.Log("Adding " + maxFakeDoors + " fake doors! Current hall count: "+halls.Count);

        Vector2 fakeBounds_x    = new Vector2(0, cells_ad.x);
        Vector2 fakeBounds_y    = new Vector2(0, cells_ad.y);
        Vector3 realPos         = Vector3.zero;
        testPos                 = Vector2.zero;

        int attempts2 = 0;
        while (fakeDoors < maxFakeDoors && attempts2 < 10) {
            attempts = 0;
            while (attempts < 10) {
                bool succeeded = true;
                testPos = new Vector2(Mathf.Round(Random.Range(fakeBounds_x[0], fakeBounds_x[1] * 2)), Mathf.Round(Random.Range(fakeBounds_y[0], fakeBounds_y[1] * 2)));
                testPos = testPos / 2;

                /*offset.x = Mathf.Round(Random.Range(-1, 1));
                if ((offset.x == -1 && testPos.x == 0) || (offset.x == 1 && testPos.x == cells.x - 1)) offset.x = -offset.x;
                if (offset.x == 0) offset.y = Mathf.Round(Random.Range(-1, 1));
                if ((offset.y == -1 && testPos.y == 0) || (offset.y == 1 && testPos.y == cells.y - 1)) offset.y = 0;*/

                //testPos += offset / 2;

                realPos = startpos + new Vector3(testPos.x * (layoutBounds.x / cells.x), 0, testPos.y * (layoutBounds.z / cells.y));

                // look if the space is free
                foreach (GameObject go in halls) {
                    if (go.transform.position == realPos) {
                        succeeded = false;
                    }
                }

                // sanity check: hallways should not be placed in the center of a room, or on the diagonal
                // this statement makes the placement fail if both position values are decimal (ie. on the diagonal axis) or if both are whole nums (ie. on the whole room axis)
                if (MathExt.IsWholeNum(testPos.x) == MathExt.IsWholeNum(testPos.y)) {
                    succeeded = false;
                }

                // only create the hallway if succeeded
                if (succeeded) {
                    GameObject hw = Instantiate(room_hallways[1], transform);

                    hw.transform.position = realPos;
                    hw.name = "Hallway (Fake) (" + testPos.x + ", " + testPos.y + ")";

                    halls.Add(hw);

                    fakeDoors++;
                } else {
                    attempts++;
                }
            }
            /*for (var i = 0; i < fakes.Count; i++) {
                halls.Remove(fakes[i]);
                Destroy(fakes[i]);
            }*/
            attempts2++;
        }

        Debug.Log("Finished creating " + fakeDoors + " fake doors!");
    }

    void GenerateStep(Vector2 position, List<GameObject> visited, List<GameObject> hallways, GameObject[,] allrooms, GameObject unentrance) {
        int pleasedontblowup = 0;
        Vector2 offset = Vector2.zero;
        Vector3 startpos = transform.position - new Vector3(layoutBounds.x / 2, 0, layoutBounds.z / 2);//= new Vector2(Mathf.Round(Random.Range(0f, 1f)) * (cells.x-1), Mathf.Round(Random.Range(0f, 1f)) * (cells.y-1));

        visited.Add(allrooms[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)]);

        if (visited.Contains(unentrance)) return;

        while (pleasedontblowup < 10) {
            offset.x = Mathf.Round(Random.Range(-1, 1));
            if ((offset.x == -1 && position.x == 0) || (offset.x == 1 && position.x == cells.x - 1)) offset.x = -offset.x;
            if (offset.x == 0) offset.y = Mathf.Round(Random.Range(-1, 1));
            if ((offset.y == -1 && position.y == 0) || (offset.y == 1 && position.y == cells.y - 1)) offset.y = 0;//-offset.y;

            position += offset;
            if (offset != Vector2.zero && !visited.Contains( allrooms[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)] ) ) {
                position -= offset/2;

                GameObject hallway = Instantiate(room_hallways[0], transform);
                float r = position.x;
                float c = position.y;

                hallway.transform.position = startpos + new Vector3(c * (layoutBounds.x / cells.x), 0, r * (layoutBounds.z / cells.y));
                hallway.name = "Hallway (" + position.x + ", " + position.y + ")";

                hallways.Add(hallway);

                position += offset / 2;

                GenerateStep(position, visited, hallways, allrooms, unentrance);
                return;
            } else {
                pleasedontblowup++;

                position -= offset;
            }
        }
    }

    void ResetMaze() {
        for (var i = 0; i < transform.childCount; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
        Generate();
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;

        Vector3 bounds = new Vector3(layoutBounds.x * transform.localScale.x, layoutBounds.y * transform.localScale.y, layoutBounds.z * transform.localScale.z);

        Gizmos.DrawWireCube(transform.position, (bounds) );
    }
}
