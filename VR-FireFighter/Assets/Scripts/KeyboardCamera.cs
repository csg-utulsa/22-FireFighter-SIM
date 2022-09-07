using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 screenPoint = GetComponent<Camera>().ScreenToWorldPoint( new Vector3(mousePos.x, mousePos.y, GetComponent<Camera>().nearClipPlane) );
        transform.LookAt(screenPoint);
    }
}
