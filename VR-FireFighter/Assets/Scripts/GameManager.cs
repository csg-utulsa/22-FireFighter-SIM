using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // setup
    [Header("Game Properties")]
    [Tooltip("Time the player has to rescue stuff. Measured in minutes, but uses a float for extra control.")]
    public float gameProps_rescueTime = 3;
    public static float gameProps_rescueTime_static;

    public static float timeRemaining_mins = 0;
    public static float timeRemaining_seconds = 0;

    UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        gameProps_rescueTime_static = gameProps_rescueTime;

        StartGame();
    }

    public static void StartGame() {
        timeRemaining_mins = Mathf.Floor(gameProps_rescueTime_static)-1;
        timeRemaining_seconds = (gameProps_rescueTime_static - (Mathf.Floor(gameProps_rescueTime_static)-1)) * 60;
        Debug.Log("static: " + gameProps_rescueTime_static);
        Debug.Log("time is: " + timeRemaining_mins + ":" + timeRemaining_seconds);
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining_seconds -= Time.deltaTime;
        if (timeRemaining_seconds < 0) {
            timeRemaining_mins--;
            timeRemaining_seconds = 60;

            Debug.Log("time is now: " + timeRemaining_mins);
        }
        uiManager.UpdateUI();
    }
}
