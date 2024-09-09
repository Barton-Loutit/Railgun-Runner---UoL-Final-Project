/*
 * GameSessionManager manages the game's lifetime. A regular game would have a 
 * less persistent runtime where a player would regularly close and startup the
 * application (so most teardown can wait until closing, as opposed to just
 * returning to the main menu).
 * 
 * Since this is an arcade game, GSM includes methods to reinitialize a play
 * session when the player comes back to the menu. Therefore, the "Game Session"
 * is effectively from pressing start through to returning to the menu.
 * 
 * GSM manages the canvas display for score, bombs, continue, text.
 * GSM also manages initialization through a GSM instance, which
 * is responsible for intializing a new round or a new session when appropriate.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSessionManager : MonoBehaviour
{
    //setup gameSessionManager instance for use in singleton, assign canvas elements
    //in editors to manage displays.
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text bombsText;
    [SerializeField] TMP_Text continueText;
    [SerializeField] TMP_Text timerText;

    public static GameSessionManager gameSessionManagerInstance;
    
    //Reference to the countdown timer to diplay timerText.
    public Timer timer;
    //I'm almost certain that this assignment doesn't need to be here, but I'm
    //too afraid to remove it.
    public PlayerController playerController;

    //Setup the singleton - if there is no existing instance assigned,
    //then this instance should be used as the singleton, and persist
    //across scene transitions. Else, this should be destroyed.   
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

    //Don't show continue and timerText at the start
    //Assign the playercontroller that I'm scared to remove.
    void Start()
    {
        continueText.enabled = false;
        timerText.enabled = false;
        playerController=FindObjectOfType<PlayerController>();
    }

    //Setter for score text, called when score is updated.
    public void SetScoreText(string newScoreText)
    {
        scoreText.text = newScoreText;
    }

    //Setter for bomb text, called when bomb count is updated.
    public void SetBombsText(string newBombsText)
    {
        bombsText.text = newBombsText;
    }

    //Setter for timer (duration) text, called when the player dies
    //and every frame thereafter until either:
    //  the timer lapses and we return to the menu
    //  the player presses spacebar to reload the level
    public void SetTimerText(string newTimerText)
    {
        timerText.text = newTimerText;
    }

    //When called, will toggle display of continue elements.
    public void ToggleCountdownDisplayActive(bool countdownState)
    {
        continueText.enabled = countdownState;
        timerText.enabled = countdownState;
    }

    //Obfuscating timer class via this public GSM to have more access control
    //over timer.
    //Called by collisionManager when player dies.
    public void StartCountdownTimer()
    {
        timer.BeginCountdown();
    }
    
    // When a new round starts, make sure the timer has been reset, display
    //is inactive, statmanager values (score, bombs) are cleared, and
    //reset the text.
    public void InitializeNewRound()
    {
        timer.ResetCountdownTimer();
        this.ToggleCountdownDisplayActive(false);
        PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerValues();
        PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerText();
    }

    //When a new session starts (i.e. when we return to the main menu), stop
    //displaying the countdown display, reset the playerStat manager, destroy it,
    //destroy this GSM, and restart the game (effectively with a new GSM).
    public void initializeNewSession()
    {
        this.ToggleCountdownDisplayActive(false);
        if (PlayerStatManager.playerStatManagerInstance != null)
        {
            PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerValues();
            PlayerStatManager.playerStatManagerInstance.ResetPlayerStatManagerText();
            Destroy(PlayerStatManager.playerStatManagerInstance.gameObject);
        }
        
        Destroy(gameSessionManagerInstance.gameObject);
        SceneHandler.sceneHandlerInstance.RestartGame();
    }

    //Obfuscating timer class via this public GSM to have more access control
    //over timer.
    //Called by playerController to handle "spacebar" to reload level when dead.
    public bool isTimerCountingDown()
    {
        return timer.isTimerCountingDown();
    }

}
