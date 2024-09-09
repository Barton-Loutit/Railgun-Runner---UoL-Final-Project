/*
 * This Timer class is responsible for handling a continue countdown
 * when the player crashes. This is also responsible for returning
 * the status of the timer.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] float remainingTimerDuration = 60;
    float initialTimerDuration;
    bool timerLapsed = false;
    bool countingDown = false;
    [SerializeField] UnityEvent timerIsLapsed;

    private void Start()
    {
        initialTimerDuration = remainingTimerDuration;
    }


    // If the timer has been started, check if there is still time left.
    // If there is still time left, update the time accordingly.
    // If there is not time left, set the related states.
    // Update the timer values on the canvas.
    // Once the timer has lapsed, trigger an event reinitializing
    // the game session (as the timer being lapsed is the trigger
    // for returning to the main menu).
    void FixedUpdate()
    {
        if (countingDown)
        {
            if (remainingTimerDuration > 0)
            {
                remainingTimerDuration -= Time.deltaTime;
            }
            else
            {
                remainingTimerDuration = 0;
                timerLapsed = true;
                countingDown = false;
            }
            GameSessionManager.gameSessionManagerInstance.SetTimerText(getTimerValue().ToString());
        }
        if (timerLapsed)
        {
            timerIsLapsed.Invoke();
        }
    }

    //Getter for timerLapsed state
    //Not currently used, but used for testing.
    public bool isTimerLapsed()
    {
        //OnEvent timerLapsed: ReturnToMenu
        return timerLapsed;
    }

    // Getter for timer state.
    public bool isTimerCountingDown()
    {
        return countingDown;
    }

    //Start the timer, set the state of the timer to be "countingDown" (which allows 
    //for the user to press "space" to restart the level in PlayerController).
    public void BeginCountdown()
    {
        GameSessionManager.gameSessionManagerInstance.ToggleCountdownDisplayActive(true);
        countingDown = true;

    }

    //Returns an integer value of the amount of time reimaining in the timer.
    public float getTimerValue()
    {
        return Mathf.RoundToInt(remainingTimerDuration);
    }

    //Reinitializes the session when returning to menu
    //Not currently in use, but used for testing.
    public void ReturnToMenu()
    {
        GameSessionManager.gameSessionManagerInstance.initializeNewSession();
    }

    //Reintiializes the values of the timer.
    public void ResetCountdownTimer()
    {
        remainingTimerDuration = initialTimerDuration;
        timerLapsed = false;
        countingDown = false;
    }
}

