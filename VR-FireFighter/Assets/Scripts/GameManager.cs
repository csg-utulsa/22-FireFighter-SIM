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
    static UI_Typewritter uiObjective;
    static UI_Typewritter uiGameOver;
    static RG_Spawns mgSpawns;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiObjective = GameObject.Find("ObjectiveText").GetComponent<UI_Typewritter>();
        uiGameOver = GameObject.Find("TimeUpText").GetComponent<UI_Typewritter>();
        
        mgSpawns = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<RG_Spawns>();

        gameProps_rescueTime_static = gameProps_rescueTime;

        Invoke("Start2", 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        // do timer stuff
        timeRemaining_seconds -= Time.deltaTime;
        if (timeRemaining_seconds < 0) {
            timeRemaining_mins--;
            timeRemaining_seconds = 60;

            Debug.Log("time is now: " + timeRemaining_mins);
        }
        // update the ui
        uiManager.UpdateUI();

        // check for gameover
        if (timeRemaining_mins <= 0 && timeRemaining_seconds <= 0) {
            uiGameOver.gameObject.SetActive(true);
            uiGameOver.StartTypewrite();
        }

        // take user input for things
        GetInputs();
    }

    // for user stuff
    void GetInputs() {
        // take restart input
        if (Input.GetKeyDown(KeyCode.F5)) {
            StartGame();
        }
    }
    
    void Start2() {
        StartGame();
    }

    // starts the game and whatnot
    public static void StartGame() {
        timeRemaining_mins = Mathf.Floor(gameProps_rescueTime_static) - 1;
        timeRemaining_seconds = (gameProps_rescueTime_static - (Mathf.Floor(gameProps_rescueTime_static) - 1)) * 60;
        Debug.Log("static: " + gameProps_rescueTime_static);
        Debug.Log("time is: " + timeRemaining_mins + ":" + timeRemaining_seconds);

        mgSpawns.SpawnPlayer();
        mgSpawns.SpawnFires();
        mgSpawns.SpawnRescues();

        // setup ui stuff
        uiObjective.gameObject.SetActive(true);
        uiObjective.StartTypewrite();
        uiGameOver.gameObject.SetActive(false);
    }
}
