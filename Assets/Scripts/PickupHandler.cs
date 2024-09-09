/*
 * This class is responsible for managing any pickups in the game.
 * 
 * In the current state this is the bombs, in the future state this could include
 * shield, health, different ammo types, etc.
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHandler : MonoBehaviour
{
    //Essentially how much to increment pickup class.
    [SerializeField] int numPickupsToAdd = 1;

    //If the thing triggering the pickup is tagged "Player",
    //Update accordingly, play the SFX, and destroy this pickup object.
    
    //In the future, a switch could be used to define which
    //class of pickup to increment.
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            PlayerStatManager.playerStatManagerInstance.updateBombs(numPickupsToAdd);
            AudioManager.audioManagerInstance.Play("PickupCollectionSFX");
            Destroy(this.gameObject);
        }
    }
}
