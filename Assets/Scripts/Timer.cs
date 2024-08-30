using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] float remainingTimerDuration = 60;
    bool timerLapsed = false;
    bool countingDown = false;
    TimerTextManager timerTextManager;

    // Start is called before the first frame update
    void Start()
    {
        timerTextManager = FindObjectOfType<TimerTextManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FixedUpdate()
    {
        if (countingDown)
        {
            if(remainingTimerDuration > 0)
            {
                remainingTimerDuration -= Time.deltaTime;
            }
            else
            {
                remainingTimerDuration = 0;
                timerLapsed = true;
            }
            timerTextManager.UpdateTimerText();
        }
    }

    public bool isTimerLapsed()
    {
        return timerLapsed; 
    }


    public void BeginCountdown()
    {
        timerTextManager.SetTextRendering(true);
        countingDown = true;
    }
    
    public float getTimerValue()
    {
        return Mathf.RoundToInt(remainingTimerDuration);
    }
}
