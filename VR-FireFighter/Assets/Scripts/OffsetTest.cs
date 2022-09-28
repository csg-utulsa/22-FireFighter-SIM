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
        Invoke("ResetPos", 0.5f);
    }

    void ResetPos() {
        /*handLeft = transform.GetChild(1).GetChild(3);
        handRight = transform.GetChild(2).GetChild(3);

        if (handLeft == null) {
            Invoke("ResetPos", 0.5f);
            return;
        }
        Vector3 newPosL = Vector3.zero; //new Vector3(-0.125f/2, 0, 0);
        Vector3 newPosR = Vector3.zero;//new Vector3(0.125f/2, 0, 0);

        handLeft.localPosition = newPosL;
        handRight.localPosition = newPosR;

        transform.GetChild(1).GetChild(1).localPosition = newPosL;
        transform.GetChild(1).GetChild(2).localPosition = newPosL;

        transform.GetChild(2).GetChild(1).localPosition = newPosR;
        transform.GetChild(2).GetChild(2).localPosition = newPosR;*/
    }

    public void Feedback(string str) {
        HelpTester.ShowFeedback(str);
    }
}
