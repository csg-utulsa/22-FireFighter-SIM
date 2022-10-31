using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Random;

//WIP Code for Randomly Spreading Fire

public class FireSpread : MonoBehaviour
{
    public bool spreading = true;
    public int spreadChance = 50;
    int random;
    //Random generator = new Random();
    GameObject spreadFire;
    //Vector3 = 

    // Update is called once per frame
    void Update()
    {
        if (spreading)
        {
           // random = generator.Next(100);
            if(random <= spreadChance)
            {
             //   Instantiate(spreadFire)
            }

        }
    }
}
