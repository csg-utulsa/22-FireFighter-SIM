using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Pulse : MonoBehaviour
{
    // vars
    [Header("Color Properties")]
    public Color[] colorCycle;
    public float timeBetweenColors = 1f;


    float colorTime = 0;
    int colorState = 0;

    float timeBetween_stored = 1f;

    TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timeBetween_stored = timeBetweenColors;
    }

    // Update is called once per frame
    void Update()
    {
        text.color = Color.Lerp(text.color, colorCycle[colorState], colorTime / timeBetweenColors);
        colorTime += Time.deltaTime;
        if (text.color == colorCycle[colorState] || colorTime >= timeBetweenColors) {
            text.color = colorCycle[colorState];

            colorState++;
            colorTime = 0;

            if (colorState >= colorCycle.Length) {
                colorState = 0;
            }
        }
    }

    public void ResetColor() {
        colorState = 0;
        text.color = colorCycle[colorState];
        timeBetweenColors = timeBetween_stored;
    }
}
