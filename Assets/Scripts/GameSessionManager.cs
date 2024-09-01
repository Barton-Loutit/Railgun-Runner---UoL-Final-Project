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
    public PlayerController playerController;
    //Instantiating as singleton
    void Awake()
    {
        if (gameSessionManagerInstance == null)
        {
            gameSessionManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
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
        playerController=FindObjectOfType<PlayerController>();
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
    public void InitializeNewRound()
    {
        timer.ResetCountdownTimer();
        this.ToggleCountdownDisplayActive(false);
        PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerValues();
        PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerText();
        //playerController.EnableHandlingControls();
    }

    public void initializeNewSession()
    {
        this.ToggleCountdownDisplayActive(false);
        if (PlayerStatManager.playerStatManagerInstance != null)
        {
            PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerValues();
            PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerText();
            Destroy(PlayerStatManager.playerStatManagerInstance.gameObject);
        }
        
        
        //Destroy(AudioManager.audioManagerInstance.gameObject);
        Destroy(gameSessionManagerInstance.gameObject);
        SceneHandler.sceneHandlerInstance.RestartGame();


        //Go back to main menu
        //Set score to zero
        //Set bomb count to zero
        //Teardown PlayerStatManager
    }

    public bool isTimerCountingDown()
    {
        return timer.isTimerCountingDown();
    }

}
