using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float remainingTimerDuration = 60;
    bool timerLapsed = false;
    bool countingDown = false;

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
            }
            GameSessionManager.gameSessionManagerInstance.SetTimerText(getTimerValue().ToString());

        }
    }

    public bool isTimerLapsed()
    {
        return timerLapsed;
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
}

