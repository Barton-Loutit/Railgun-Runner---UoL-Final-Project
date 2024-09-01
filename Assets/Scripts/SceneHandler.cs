using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler sceneHandlerInstance { get; private set; }

    void Awake()
    {
        if(sceneHandlerInstance == null)
        {
            sceneHandlerInstance = this;
            DontDestroyOnLoad(sceneHandlerInstance);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    void OnEnable()
    {

        //reloadLevel.Enable();
        //loadNextLevel.Enable();
    }

    void OnDisable()
    {

        //reloadLevel.Disable();
        //loadNextLevel.Disable();
    }

    void Update()
    {
        //if(reloadLevel.action.ReadValue<float>() == 1)
/*        if (reloadLevel.action.ReadValue<float>() == 1)
        {
            this.RestartLevel();
        }*/
        //Disabling until I fix inputAction mapping
        /*else if(loadNextLevel.action.ReadValue<float>() == 1)
        {
            this.LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }*/
    }

    //levelToLoad correlates with build index;
    public void LoadLevel(int levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad);
    }

    //Scene 0 is the start menu
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
