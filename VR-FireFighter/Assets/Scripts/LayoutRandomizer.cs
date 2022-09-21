using System.Collections;
using System.Collections.Generic;
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
        Vector2 startPos = new Vector2( Mathf.Round(Random.Range(0, 1)) * cells.x, Mathf.Round(Random.Range(0, 1)) * cells.y );
        Vector2 endPos = new Vector2(-1, -1);
        float expectedProximity = 7;
        int attempts = 0;
        while ( (startPos == endPos || endPos == (Vector2.one * -1.0f) ) && attempts < 10) {
            // generate an end position

            // determine side to place the exit on
            int sideSeed = Mathf.RoundToInt( Random.Range(0, 3) );
            if (sideSeed > 0 && sideSeed < 3) {
                endPos = new Vector2( sideSeed - 1, Mathf.Round(Random.Range(0, 1)) * cells.y );
            } else {
                endPos = new Vector2( Mathf.Round(Random.Range(0, 1)) * cells.y, Mathf.Round(sideSeed / 3) );
            }

            // check for proximity
            if (!(Vector2.Distance(startPos, endPos) >= expectedProximity) ) {
                Debug.Log("startPos: "+startPos+" | "+endPos+" | proximity: "+Vector2.Distance(startPos, endPos));

                endPos = new Vector2(-1, -1);
            } else {
                Debug.Log("found a good exit location!");
            }

            attempts++;
        }

        // now go thru and try to add doors
        for (var r = 0; r < cells.x; r++) {
            for (var c = 0; c < cells.y; c++) {

            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, (layoutBounds) );
    }
}
