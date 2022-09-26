using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
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
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
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
                GameObject entrance = Instantiate(room_exit[0], transform);

                entrance.transform.position = oldPos;
                entrance.name = "Enter " + oldEnt.name;
                entrance.transform.SetSiblingIndex(oldEnt.transform.GetSiblingIndex());

                rooms[(int)startPos.x, (int)startPos.y] = entrance;
                Destroy(oldEnt);


                // calling it unentrance b/c exit is a reserved keyword
                oldEnt                  = rooms[(int)endPos.x, (int)endPos.y];
                oldPos                  = oldEnt.transform.position;
                GameObject unentrance   = Instantiate(room_exit[1], transform);

                unentrance.transform.position = oldPos;
                unentrance.name = "Exit " + oldEnt.name;
                unentrance.transform.SetSiblingIndex(oldEnt.transform.GetSiblingIndex());

                rooms[(int)endPos.x, (int)endPos.y] = unentrance;
                Destroy(oldEnt);
            }

            attempts++;
        }

        //rooms_sealed = rooms;

        // now go thru and try to add doors
        Vector2 testPos = new Vector2(startPos.x, startPos.y);
        Vector2 offset  = Vector2.zero;
        List<GameObject> rooms_visited = new List<GameObject>();
        List<GameObject> halls = new List<GameObject> ();
        bool exitfound  = false;
        attempts = 0;

        GenerateStep(testPos, rooms_visited, halls, rooms);
        /*while (!exitfound && attempts < 10) {
            int pleasedontblowup = 0;

            if (testPos == endPos) {
                exitfound = true;
            } else if (rooms_sealed.Count >= ((int)cells.x * (int)cells.y)) {
                rooms_sealed = new List<GameObject>();
                testPos = new Vector2(startPos.x, startPos.y);

                for (var i = halls.Length - 1; i >= 0; i--) {
                    Destroy(halls[i]);
                }
                halls = new GameObject[(int)cells.x * (int)cells.y];

                Debug.Log("not a viable path, moving on to attempt #"+(attempts+1));
                attempts++;
            } else {
                while (pleasedontblowup < 10) {
                    offset.x = Mathf.Round(Random.Range(-1, 1));
                    if ((offset.x == -1 && testPos.x == 0) || (offset.x == 1 && testPos.x == cells.x - 1)) offset.x = -offset.x;
                    if (offset.x == 0) offset.y = Mathf.Round(Random.Range(-1, 1));
                    if ((offset.y == -1 && testPos.y == 0) || (offset.y == 1 && testPos.y == cells.y - 1)) offset.y = 0;//-offset.y;
                    Debug.Log("offset: " + offset);

                    if (offset == Vector2.zero && !rooms_sealed.Contains(rooms[Mathf.RoundToInt(testPos.x), Mathf.RoundToInt(testPos.y)])) {
                        GameObject hallway = Instantiate(room_hallways[0], transform);
                        hallway.transform.position = rooms[Mathf.RoundToInt(testPos.x), Mathf.RoundToInt(testPos.y)].transform.localPosition + new Vector3(offset.x, 0f, offset.y);
                        hallway.name = "Hallway (" + (testPos.x + (offset.x / 2)) + ", " + (testPos.y + (offset.y / 2)) + ")";

                        Debug.Log("testPos: " + testPos);
                        rooms_sealed.Add(rooms[Mathf.RoundToInt(testPos.x), Mathf.RoundToInt(testPos.y)]);

                        testPos += offset;
                        Debug.Log("made a hallway at " + testPos);
                    } else {
                        pleasedontblowup++;
                    }
                } 
                if (pleasedontblowup >= 10) {
                    Debug.Log("it blew up");
                    attempts++;
                }
            }
        }*/
    }

    void GenerateStep(Vector2 position, List<GameObject> visited, List<GameObject> hallways, GameObject[,] allrooms) {
        int pleasedontblowup = 0;
        Vector2 offset = Vector2.zero;
        Vector2 startPos = transform.position - new Vector3(layoutBounds.x / 2, 0, layoutBounds.z / 2);//= new Vector2(Mathf.Round(Random.Range(0f, 1f)) * (cells.x-1), Mathf.Round(Random.Range(0f, 1f)) * (cells.y-1));

        visited.Add(allrooms[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y)]);

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

                hallway.transform.position = new Vector3( startPos.x, 0, startPos.y ) + new Vector3(c * (layoutBounds.x / cells.x), 0, r * (layoutBounds.z / cells.y));
                hallway.name = "Hallway (" + position.x + ", " + position.y + ")";

                hallways.Add(hallway);

                position += offset / 2;

                GenerateStep(position, visited, hallways, allrooms);
                return;
            } else {
                pleasedontblowup++;

                position -= offset;
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, (layoutBounds) );
    }
}
