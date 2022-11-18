/** 
 * Author: Akram Taghavi-Burris
 * Created: 11-15-22
 * Modified: 
 * Description: functions for downed firefighter
 **/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownedFighterFighter : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip audioClip;
    public float audioDelay;

    // Start is called before the first frame update
    void Start()
    {

       audioSource = GetComponent<AudioSource>();
       Debug.Log(audioSource);
       audioClip = audioSource.clip;
       Debug.Log(audioClip);
        audioSource.PlayDelayed(audioDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
