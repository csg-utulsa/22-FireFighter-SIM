using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSwapper : MonoBehaviour
{
    public GameObject keyboardCamera;
    public GameObject vrCamera;

    void Awake() {
        #if UNITY_STANDALONE_WIN || UNITY_EDITOR
                //keyboardCamera.SetActive(true);
                //vrCamera.SetActive(false);
        #elif ENABLE_VR
                //vrCamera.SetActive(true);
                //keyboardCamera.SetActive(false);
        #endif
    }
}
