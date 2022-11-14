using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Typewritter : MonoBehaviour
{
    // vars
    [Header("Typing")]
    [Tooltip("How many letters are typed every second.")]
    public float charsPerSecond = 5;
    [Tooltip("How long typing will wait after hitting a special character (in seconds). Use -1 for no pauses!")]
    public float pauseOnSpecialCharacters = 1f;
    [Tooltip("Whether or not to pause on spaces as if they were punctuation. Ignore if pauseOnSpecialCharacters is already -1.")]
    public bool pauseOnBlanks = false;
    [Space(10)]
    [Header("AutoFade")]
    [Tooltip("How long the text will wait after finishing typing before beginning fading out (in seconds). Use -1 for no autofade!")]
    public float waitForFadeOut = -1;
    [Tooltip("How long a fadeout will take (in seconds). Ignore if waitForFadeOut is already -1.")]
    public float fadeOutTime = 3f;

    string txt;
    string txtprog = "";

    float wait = 0;
    float fadeAlpha = 1;
    bool fadeOut = false;

    [HideInInspector]
    public TextMeshProUGUI textobj;


    // Start is called before the first frame update
    void Start()
    {
        // grab references
        textobj = GetComponent<TextMeshProUGUI>();

        // start typewriting
        StartTypewrite();
    }

    // Update is called once per frame
    void Update()
    {
        // manage fade effect
        if (fadeOut) {
            // decrement alpha
            fadeAlpha -= (1 / fadeOutTime)*Time.deltaTime;

            // check if done
            if (fadeAlpha <= 0) {
                // stop doing stuff
                fadeOut = false;
                fadeAlpha = 1;
                wait = 0;

                // deactivate
                textobj.alpha = 1;
                textobj.gameObject.SetActive(false);
            } else {
                // we aren't done, so update visuals
                textobj.alpha = fadeAlpha;
            }
        }
    }

    // starts the typewritter
    public void StartTypewrite() {
        // fill the vars necessary
        txt = textobj.text;
        txtprog = "";

        // clear the original string just so we don't see weird after images
        //textobj.text = "";

        // start calling the progress function
        InvokeRepeating("WriteString", 1 / charsPerSecond, 1 / charsPerSecond);
    }

    // types stuff, probably
    void WriteString() {
        // check for wait
        if (wait > 0) {
            wait -= 1 / charsPerSecond;
            return;
        } else {
            wait = 0;
        }

        // grab the letter
        int index = txtprog.Length;
        string addchar = txt.Substring(index, 1);

        // add a pause if relevant
        if (pauseOnSpecialCharacters != -1f && Stringext.StringIsSpecialChar(addchar, pauseOnBlanks)) {
            wait = pauseOnSpecialCharacters;
        }

        // add the character
        txtprog += addchar;

        // update the text
        if (txtprog != txt) textobj.text = txtprog;
        else {
            // update the string, one final time
            textobj.text = txt;
            txtprog = "";

            // suppose we're done then
            CancelInvoke("WriteString");

            // check if an autofade should happen
            if (waitForFadeOut != -1f) {
                // start wait until fade if applic
                Invoke("StartFade", waitForFadeOut);
            }
        }
    }

    // i'm making an invoke function for a single boolean toggle, and nobody can stop me!
    void StartFade() {
        fadeOut = true;
    }
}
