using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI rescueText;

    public bool victoryState = false;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI() {
        int rescCount = GameObject.FindGameObjectsWithTag("RescueEntity").Length;

        timeText.text = string.Format("{0}:{1}", GameManager.timeRemaining_mins, Stringext.NumberPad( Mathf.FloorToInt(GameManager.timeRemaining_seconds), 2)  );
        rescueText.text = ""+rescCount;

        // check for victory conditions
        if (!victoryState && rescCount <= 0) {
            victoryState = true;
            GameManager.WinGame();
            gm.Invoke("TryAgainPopup", 2f);
        }
    }

    public void ShowGameOverUI() {

    }
}
