using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestToggle : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle() {
        rb.isKinematic = !rb.isKinematic;
    }
}