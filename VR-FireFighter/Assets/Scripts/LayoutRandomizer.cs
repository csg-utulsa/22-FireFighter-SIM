using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;


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
        GameObject[,] rooms_sealed  = new GameObject[(int)cells.x, (int)cells.y];
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
        while ( (startPos == endPos || endPos == (Vector2.one * -1.0f) ) && attempts < 10) {
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

        rooms_sealed = rooms;

        // now go thru and try to add doors
        Vector2 testPos = new Vector2(startPos.x, startPos.y);
        bool exitfound = false;
        attempts = 0;
        /*while (!exitfound && attempts < 20) {
            if (testPos == endPos) {

            }
        }*/
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, (layoutBounds) );
    }
}
