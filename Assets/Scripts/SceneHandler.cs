using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler sceneHandlerInstance { get; private set; }
    [SerializeField] InputAction reloadLevelAction;
    [SerializeField] InputAction loadNextLevelAction;

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

    void OnEnable()
    {
        reloadLevelAction.Enable();
        loadNextLevelAction.Enable();
    }

    void OnDisable()
    {
        reloadLevelAction.Disable();
        loadNextLevelAction.Disable();
    }

    //levelToLoad correlates with build index;
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

    //Scene 0 is the start menu
    public void RestartGame()
    {
        /*GameSessionManager.gameSessionManagerInstance.initializeNewSession();*/
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartGame(int Level1SceneNumber)
    {
        SceneManager.LoadScene(1);
    }
}
