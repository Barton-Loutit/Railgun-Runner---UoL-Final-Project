using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneHandler.sceneHandlerInstance.LoadLevel(1);
    }

    public void QuitGame()
    {
        //Functionality works for standalone .exe build
        Debug.Log("Quit");
        Application.Quit();
    }

}
