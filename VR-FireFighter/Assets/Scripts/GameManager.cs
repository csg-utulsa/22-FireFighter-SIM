using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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

    static UIManager uiManager;
    static UI_Typewritter uiObjective;
    static UI_Typewritter uiGameOver;
    static UI_Typewritter uiWin;
    static UI_Typewritter uiTryAgain;
    static RG_Spawns mgSpawns;

    public static bool timerActive = true;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        uiObjective = GameObject.Find("ObjectiveText").GetComponent<UI_Typewritter>();
        uiGameOver = GameObject.Find("TimeUpText").GetComponent<UI_Typewritter>();
        uiWin = GameObject.Find("VictoryText").GetComponent<UI_Typewritter>();
        uiTryAgain = GameObject.Find("TryAgainText").GetComponent<UI_Typewritter>();
        
        mgSpawns = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<RG_Spawns>();

        gameProps_rescueTime_static = gameProps_rescueTime;

        Invoke("Start2", 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Update");

        // take user input for things
        GetInputs();

        // exit if time up
        if (!timerActive)
        {
            Debug.Log("don't count rn");
            return;
        }

        // check for gameover
        if ((timeRemaining_mins <= 0 && timeRemaining_seconds <= 0) || timeRemaining_mins < 0) {
            // call gameover method
            GameOver();
            // quit counting
            return;
        }

        // do timer stuff
        timeRemaining_seconds -= Time.deltaTime;
        Debug.Log("time remaining secs: " + timeRemaining_seconds);
        if (timeRemaining_seconds < 0) {
            timeRemaining_mins--;
            timeRemaining_seconds = 60;

            Debug.Log("time is now: " + timeRemaining_mins);
        }
        // update the ui
        uiManager.UpdateUI();

        Debug.Log("Update End");


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
        timerActive = true;

        // reset rescues
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("RescueEntity") ) {
            Destroy(go);
        }

        mgSpawns.SpawnPlayer();
        mgSpawns.SpawnFires();
        mgSpawns.SpawnRescues();

        // setup ui stuff
        uiObjective.gameObject.SetActive(true);
        uiObjective.StartTypewrite();
        uiGameOver.gameObject.SetActive(false);
        uiWin.gameObject.SetActive(false);
        uiTryAgain.gameObject.SetActive(false);

        // reset special effects
        //uiManager.timeText.GetComponent<UI_Pulse>().ResetColor();
        //uiManager.timeText.GetComponent<UI_Pulse>().enabled = false;
    }

    public static void WinGame() {
        // set ui stuff
        uiWin.gameObject.SetActive(true);
        uiWin.StartTypewrite();

        // disable timer
        timerActive = false;

        // reset special effects
        //uiManager.timeText.GetComponent<UI_Pulse>().ResetColor();
        //uiManager.timeText.GetComponent<UI_Pulse>().enabled = false;
    }

    public void GameOver() {
        // enable gameover
        uiGameOver.gameObject.SetActive(true);
        uiGameOver.StartTypewrite();
        timerActive = false;

        Debug.Log("Gameover!");

        // set time until invoke
        Invoke("TryAgainPopup", 2f);

        // set properties
        timeRemaining_mins = 0;
        timeRemaining_seconds = 0;

        // update the ui
        uiManager.UpdateUI();

        // reset special effects
        uiManager.timeText.GetComponent<UI_Pulse>().ResetColor();
        uiManager.timeText.GetComponent<UI_Pulse>().enabled = false;
    }

    public void TryAgainPopup() {
        uiTryAgain.gameObject.SetActive(true);
        uiTryAgain.StartTypewrite();
    }
}
