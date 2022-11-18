using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasControl : MonoBehaviour
{
    /*
     * 
     * 
     * 
     * 
     */
    // vars
    public List<Canvas> canvas = new List<Canvas>();
    public Canvas currentCanvas;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateClosestCanvas", 2f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateClosestCanvas() {
        Canvas prev = currentCanvas;
        currentCanvas = CanvasGetNearest();

        if (currentCanvas != prev) {
            prev.transform.GetChild(0).parent = currentCanvas.transform;
        }
    }

    Canvas CanvasGetNearest() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject closest = null;
        foreach (Canvas c in canvas) {
            if (closest == null || 
                Vector3.Distance(c.transform.position, player.transform.position) < 
                    Vector3.Distance(closest.transform.position, player.transform.position)) {
                closest = c.gameObject;
            }
        }
        return closest.GetComponent<Canvas>();
    }
}
