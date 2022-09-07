using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newrot = transform.eulerAngles;
        //newrot.x += (20 * Time.deltaTime);
        newrot.y += (20 * Time.deltaTime);
        transform.eulerAngles = newrot;
    }
}
