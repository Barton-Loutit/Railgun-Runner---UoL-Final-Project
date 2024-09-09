/*
 * In hindsight this should be called PlayerCollisionManager.
 * This handles the player's collision and any associated functoinality when
 * the player collides into a cliffside, for example.
 * 
 * Disables the players controls, unrenders the player mesh, plays their vfx.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    //This handles player explosion VFX
    [SerializeField] GameObject playerDeathVFX;

    //if the player collides with an object including the tag "terrain"
    //then they blow up and we handle their death.
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            HandlePlayerDeath();
        }
    }

    //When the player dies, disable their controls (weapon, movement, scene
    //management controls), play a VFX and SFX indicating their death,
    //disable their mesh, and initiate the countdown timer so the player 
    //can press "space" to restart.
    void HandlePlayerDeath()
    {
        DisablePlayerControls();
        PlayPlayerDeathVFX(playerDeathVFX);
        AudioManager.audioManagerInstance.Play("PlayerDeathSFX");
        DisablePlayerMesh();
        GameSessionManager.gameSessionManagerInstance.StartCountdownTimer();
    }

    //Collect all mesh renders in the gameObject's children (since each part
    //of this gameObject has a mesh renderer component), and disable them.
    void DisablePlayerMesh()
    {
        foreach (MeshRenderer meshRenderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    //Accesses a method in the playercontroller script, since this script isn't
    //responsible for the player inputs, and disables the events for those controls.
    
    //Also stops further collision from the player's (now invisible) ship.
    void DisablePlayerControls()
    {
        GetComponent<PlayerController>().DisableHandlingControls();
        GetComponent<BoxCollider>().enabled = false;
    }

    //Play the explosion vfx and remove the player's mesh (since they've exploded)
    void PlayPlayerDeathVFX(GameObject vfxToPlay)
    {
        GameObject vfx = Instantiate(vfxToPlay, transform.position, Quaternion.identity);

    }
}
