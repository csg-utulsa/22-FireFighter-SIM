using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetTest : MonoBehaviour
{
    public Transform handLeft;
    public Transform handRight;

    // Start is called before the first frame update
    void Start()
    {
        handLeft = transform.GetChild(1).GetChild(3);
        handRight = transform.GetChild(2).GetChild(3);

        Invoke("ResetPos", 0.5f);
    }

    void ResetPos() {
        handLeft.localPosition = new Vector3(-0.125f, 0, 0);
        handRight.localPosition = new Vector3(0.125f, 0, 0);
    }
}
