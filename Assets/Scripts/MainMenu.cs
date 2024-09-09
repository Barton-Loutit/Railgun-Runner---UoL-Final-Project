/*
 * Responsible for main menu display and scene transition to start the game.
 * Brackeys has a youtube video on Unity's canvas and menu functionalities which
 * was a helpful guide in developing the Main Menu:
 * https://www.youtube.com/watch?v=zc8ac_qUXQY
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //Need to ensure that the first level is always at buildindex 1.
        SceneHandler.sceneHandlerInstance.StartGame(1);
    }
    public void QuitGame()
    {
        //Functionality works for standalone .exe build
        Debug.Log("Quit");
        Application.Quit();
    }

}
