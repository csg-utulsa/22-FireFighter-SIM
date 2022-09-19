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
        Vector3 startpos = transform.position - new Vector3(layoutBounds.x / 2, 0, layoutBounds.z / 2);
        for (var r = 0; r < cells.x; r++) {
            for (var c = 0; c < cells.y; c++) {
                GameObject go = Instantiate(room_main[Mathf.RoundToInt(Random.Range(0,2))], transform );
                go.transform.position = startpos + new Vector3(c * (layoutBounds.x / cells.x), 0, r * (layoutBounds.z / cells.y));
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, (layoutBounds) );
    }
}
