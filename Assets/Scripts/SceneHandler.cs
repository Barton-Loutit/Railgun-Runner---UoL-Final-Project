/*
 * This class is responsible for handling all scene transitions. 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    //Setup singleton instance
    public static SceneHandler sceneHandlerInstance { get; private set; }
    
    //I used these for testing, keeping them here for future input testing.
    [SerializeField] InputAction reloadLevelAction;
    [SerializeField] InputAction loadNextLevelAction;

    //Setup the singleton - if there is no existing instance assigned,
    //then this instance should be used as the singleton, and persist
    //across scene transitions. Else, this should be destroyed.   
    void Awake()
    {
        if (sceneHandlerInstance == null)
        {
            sceneHandlerInstance = this;
            DontDestroyOnLoad(sceneHandlerInstance);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    //Enable the actions.
    void OnEnable()
    {
        reloadLevelAction.Enable();
        loadNextLevelAction.Enable();
    }
    
    //Disable the actions.
    void OnDisable()
    {
        reloadLevelAction.Disable();
        loadNextLevelAction.Disable();
    }


    //Loads the level supplied as levelToLoad, which will equal the build index to load.
    //If there levelToLoad value specified is beyond the number of scenes, return. Also,
    //if we're trying to laod the main menu, return (since we would need to reinitialize
    //the game when loading the main menu).
    //Otherwise, load that build index.
    public void LoadLevel(int levelToLoad)
    {
        /*GameSessionManager.gameSessionManagerInstance.initializeNewRound();*/
        if((levelToLoad > SceneManager.sceneCountInBuildSettings-1) || (levelToLoad == 0))
        {
            return;
        } else
        {
            SceneManager.LoadScene(levelToLoad);
        }
        
    }

    //Scene 0 is the start menu, this is called by GSM when initializing new session.
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    //Reload the scene with the same index as the current scene.
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    //Starts the game (assumes that the first level will always be at
    //build index = 1)
    public void StartGame(int Level1SceneNumber)
    {
        SceneManager.LoadScene(1);
    }
}
