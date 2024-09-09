/*
 * This script is responsible for destroying instantiated VFX gameObjects
 * created by shooting enemies, bombing enemies, or the player exploding.
 * 
 * This guarantees that the respective particleSystem will stop playing
 * after the specified time (irrespective of loop states)
 * 
 */
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Creating this class, which will get thrown on my VFX to destroy the 
// instantiated game object after the specified delay.
public class VFXDestroyer : MonoBehaviour
{
    //Specified time before destroying the attached instantiated object.
    [SerializeField] float destructionDelay = 1f;

    //Destroy after the above mentioned delay.
    //Start is called on instantiation.
    void Start()
    {
        if (!this.gameObject.IsDestroyed())
        {
            Destroy(this.gameObject, destructionDelay);
        }
    }
}
