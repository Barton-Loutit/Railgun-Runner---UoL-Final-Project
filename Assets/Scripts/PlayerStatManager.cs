/*
 * Player Stat Manager centralizes the control of all player stat elements including
 * (at the time) score and bombs, using a singleton.
 * In the future this could include health, shield value, etc.
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    //Setup the singleton instance
    public static PlayerStatManager playerStatManagerInstance { get; private set; }
    int bombCount = 0;
    int score = 0;

    //Setup the singleton - if there is no existing instance assigned,
    //then this instance should be used as the singleton, and persist
    //across scene transitions. Else, this should be destroyed.   
    void Awake()
    {
        if(playerStatManagerInstance == null)
        {
            playerStatManagerInstance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            Destroy(this.gameObject);
        }
    }

    //On start, initalize the bombs and score text.
    void Start()
    {
        GameSessionManager.gameSessionManagerInstance.SetBombsText("Bombs: " + getBombCount().ToString());
        GameSessionManager.gameSessionManagerInstance.SetScoreText("Score: " + getBombCount().ToString());
    }

    //When using a bomb or picking up bombs, update the bombs count, as well as canvas text.
    public void updateBombs(int numBombsToPickup)
    {
        bombCount += numBombsToPickup;
        GameSessionManager.gameSessionManagerInstance.SetBombsText("Bombs: " + bombCount.ToString());
    }

    //Returns the number of bombs available to the player.
    public int getBombCount()
    {
        return bombCount; 
    }

    //Increment the score by scoreValue and update the canvas text.
    public void updateScore(int scoreValue)
    {
        score += scoreValue;
        GameSessionManager.gameSessionManagerInstance.SetScoreText("Score: " + score.ToString());
    }

    //Return score (could be used on life increment, leaderboard, etc.)
    public int getScore()
    {
        return score;
    }

    //Reinitializes playerstat canvas text elements.
    public void ResetPlayerStatManagerText()
    {
        GameSessionManager.gameSessionManagerInstance.SetBombsText("Bombs: " + getBombCount().ToString());
        GameSessionManager.gameSessionManagerInstance.SetScoreText("Score: " + getBombCount().ToString());
    }

    //Reinitializes playerstat values
    public void ResetPlayerStatManagerValues()
    {
        bombCount = 0;
        score = 0;
    }
}
