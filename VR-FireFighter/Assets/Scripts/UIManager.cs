using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI() {
        timeText.text = string.Format("{0}:{1}", GameManager.timeRemaining_mins, Mathf.Floor(GameManager.timeRemaining_seconds));
    }
}
