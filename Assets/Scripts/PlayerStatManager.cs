using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager playerStatManagerinstance { get; private set; }
    //[SerializeField] int health;
    int bombCount = 0;
    int score = 0;

    void Awake()
    {
        if(playerStatManagerinstance == null)
        {
            playerStatManagerinstance = this;
            DontDestroyOnLoad(this.gameObject);
        } 
        else
        {
            Destroy(this.gameObject); 
        }
    }

    void Start()
    {
        GameSessionManager.gameSessionManagerInstance.SetBombsText("Bombs: " + getBombCount().ToString());
        GameSessionManager.gameSessionManagerInstance.SetScoreText("Score: " + getBombCount().ToString());
    }
    public void updateBombs(int numBombsToPickup)
    {
        bombCount += numBombsToPickup;
        GameSessionManager.gameSessionManagerInstance.SetBombsText("Bombs: " + bombCount.ToString());
    }

    public int getBombCount()
    {
        return bombCount; 
    }


    public void updateScore(int scoreValue)
    {
        score += scoreValue;
        GameSessionManager.gameSessionManagerInstance.SetScoreText("Score: " + score.ToString());
    }

    public int getScore()
    {
        return score;
    }
}
