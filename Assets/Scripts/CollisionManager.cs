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
    Timer countdownTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        countdownTimer = FindObjectOfType<Timer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Terrain")
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


        TempSceneManagement();
        
    }

    private void DisablePlayerMesh()
    {
        //GetComponent<MeshRenderer>().enabled = false;
        
        foreach(MeshRenderer meshRenderer in this.gameObject.GetComponentsInChildren<MeshRenderer>()) 
        { 
            meshRenderer.enabled = false;
        }
    }

    void DisablePlayerControls()
    {
        //Since this script will live on player ship, I'm disabling the
        //Player Controller script (hence decoupling the logic here), and
        //preventing further collision processing.
        GetComponent<PlayerController>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;
    }

    void PlayPlayerDeathVFX(GameObject vfxToPlay)
    {
        //Play the explosion vfx and remove the player's mesh (since they've exploded)
        GameObject vfx = Instantiate(vfxToPlay, transform.position, Quaternion.identity);
        
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void TempSceneManagement()
    {
        
        countdownTimer.BeginCountdown();
        //Current implementation doesn't perform this function
        //Keeping this code to build in functionality to reload level when a button is pressed,
        //and to check for tokens when reloading. This should be decentralized to a scene manager.
        if (countdownTimer.isTimerLapsed())
        {
            Invoke("ReloadLevel", 1f);
        }
        //This will reload the level for the player right now, but does not 
        //manage the case where a player is out of tokens.
    }
}
