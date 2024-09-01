/*
 * Brackeys has a youtube video on Unity's canvas and menu functionalities which
 * was a helpful guide in developing the Main Menu 
 * https://www.youtube.com/watch?v=zc8ac_qUXQY
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
