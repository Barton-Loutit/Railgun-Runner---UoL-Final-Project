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

    public bool isTimerLapsed()
    {
        //OnEvent timerLapsed: ReturnToMenu
        return timerLapsed;
    }

    public bool isTimerCountingDown()
    {
        return countingDown;
    }

    public void BeginCountdown()
    {
        GameSessionManager.gameSessionManagerInstance.ToggleCountdownDisplayActive(true);
        countingDown = true;

    }

    public float getTimerValue()
    {
        return Mathf.RoundToInt(remainingTimerDuration);
    }

    public void ReturnToMenu()
    {
        GameSessionManager.gameSessionManagerInstance.initializeNewSession();
    }

    public void ResetCountdownTimer()
    {
        remainingTimerDuration = initialTimerDuration;
        timerLapsed = false;
        countingDown = false;
    }
}

