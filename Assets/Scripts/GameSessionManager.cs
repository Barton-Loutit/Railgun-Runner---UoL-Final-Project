using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager gameSession { get; private set; }
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text bombsText;
    [SerializeField] TMP_Text continueText;
    [SerializeField] TMP_Text timerText;

    public static GameSessionManager gameSessionManagerInstance;
    public Timer timer;

    //Instantiating as singleton
    void Awake()
    {
        if(gameSessionManagerInstance == null)
        {
            gameSessionManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
            gameSession = this;
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        continueText.enabled = false;
        timerText.enabled = false;
    }


    public void SetScoreText(string newScoreText)
    {
        scoreText.text = newScoreText;
    }

    public void SetBombsText(string newBombsText)
    {
        bombsText.text = newBombsText;
    }

    public void SetTimerText(string newTimerText)
    {
        timerText.text = newTimerText;
    }

    public void ToggleCountdownDisplayActive(bool countdownState)
    {
        continueText.enabled = countdownState;
        timerText.enabled = countdownState;
    }

    public void StartCountdownTimer()
    {
        timer.BeginCountdown();
    }

    //To-Do
    public void initializeNewRound()
    {
        //timer.ResetCountdownTimer();
        //timer.SetTimerCountdownState();
        //ToggleCountdownDisplayActive(false);
    }

    public void initializeNewSession()
    {
        //Go back to main menu
        //Set score to zero
        //Set bomb count to zero
        //Teardown PlayerStatManager
    }

}
