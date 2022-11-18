using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RescueEntity : MonoBehaviour
{
    UIManager uiman;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("GetVars", 0.01f);
    }

    void GetVars() {
        uiman = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.tag == "Player") {
            Destroy(gameObject);
            uiman.UpdateUI();
        }
    }
}
