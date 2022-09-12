using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMatSwap : MonoBehaviour
{
    public Material mat1;
    public Material mat2;
    MeshRenderer mr;

    // Start is called before the first frame update
    void Start() {
        mr = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    public void UpdateMaterial() {
        if (mr.material != mat1) {
            mr.material = mat2;
        } else {
            mr.material = mat1;
        }
        Debug.Log("Toggled material");
    }

}