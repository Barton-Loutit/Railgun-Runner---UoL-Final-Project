using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Adding Scene Management here to enable scene reload functionality
//this should be refactored in a future release.
using UnityEngine.SceneManagement;

public class CollisionManager : MonoBehaviour
{
    //This will be the duration of the continue/insert token timer
    //[SerializeField] float continueTimer = 2f;
    //This handles player explosion VFX
    [SerializeField] GameObject playerDeathVFX;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Terrain")
        {
            Debug.Log("Literally dead right now, no cap frfr");
            HandlePlayerDeath();
        }
    }

    void HandlePlayerDeath()
    {
        DisablePlayerControls();
        PlayPlayerDeathVFX(playerDeathVFX);
        DisablePlayerMesh();
        //Trigger an event
        GameSessionManager.gameSessionManagerInstance.StartCountdownTimer();
        //NTS:Probably want to disable timeline here too
        //TempSceneManagement();

    }

    void DisablePlayerMesh()
    {
        //GetComponent<MeshRenderer>().enabled = false;

        foreach (MeshRenderer meshRenderer in this.gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
    }

    void DisablePlayerControls()
    {
        //Since this script will live on player ship, I'm disabling the
        //Player Controller script (hence decoupling the logic here), and
        //preventing further collision processing.
        GetComponent<PlayerController>().DisableHandlingControls();
        GetComponent<BoxCollider>().enabled = false;
    }

    void PlayPlayerDeathVFX(GameObject vfxToPlay)
    {
        //Play the explosion vfx and remove the player's mesh (since they've exploded)
        GameObject vfx = Instantiate(vfxToPlay, transform.position, Quaternion.identity);

    }
}
