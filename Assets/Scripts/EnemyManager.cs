using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyHitVFX;
    [SerializeField] GameObject enemyDeathVFX;
    [SerializeField] int enemyHitScoreValue = 0;
    [SerializeField] int enemyDeathScoreValue = 0;
    [SerializeField] int enemyHitPoints = 3;
    ScoreManager scoreManager;

    // Start is called before the first frame update
    void Start()
    {
        //Store the score manager singleton
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Process enemy hit on particle collision iff collision source is a "Weapon"
    //Other particles, such as other enemy missiles, may trigger this if not
    //scoped to playerweapon tag
    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "PlayerWeapon")
        {
            HandleEnemyHit();
        }
        
    }

    void HandleEnemyHit()
    {
        enemyHitPoints--;
        if(enemyHitPoints <= 0)
        {
            HandleEnemyDeath();
            
            Debug.Log("Enemy neutralized");
        }
        else
        {
            Debug.Log("Enemy hit");
            /* Adding this comment block to keep track of a design decision
             * To-Do: Add score on hit to player stats script/score manager or something
             * Note-to-self: (a GameSessionManager could implement some functinoality related to score, bombs, tokens, continue state, etc.)(?)
             * PlayerManager: Bombs, Health, 
             * ScoreManager: Score, Score retention
             * Game Session Manager: tokens, scene management, continue state
             * Canvas/UI Manager: to display data from ScoreManager, GameSessionManager, PlayerManager
             *      First To-Do:: Handle score as part of a scoreManager
             *      Second To-Do: 
            */
            scoreManager.UpdateScore(enemyHitScoreValue);
            PlayEnemyVFX(enemyHitVFX);

            //To-Do: Add SFX for enemy hit (Note-to-self: use serialized field & setup an audio manager singleton)
        }
    }
    void HandleEnemyDeath()
    {
        PlayEnemyVFX(enemyDeathVFX);
        scoreManager.UpdateScore(enemyDeathScoreValue);
        DestroyEnemyObject();
    }

    void DestroyEnemyObject()
    {
        if (!this.gameObject.IsDestroyed())
        {
            Destroy(this.gameObject);
        }
    }

    //Note: Currently VFX objects clutter the hierarchy, and gameobjects are not destroyed after they're done playing
    //may want to consider adding VFX destruction script.
    void PlayEnemyVFX(GameObject enemyVFXToPlay)
    {
        GameObject vfx = Instantiate(enemyVFXToPlay, transform.position, Quaternion.identity);
    }

    //Creating this method definition to play SFX in the future.
    void PlayEnemySFX(GameObject enemySFXToPlay)
    {
        //GameObject vfx = Instantiate(enemyVFXToPlay, transform.position, Quaternion.identity);
        //This will probably need to be done differently
    }   
}
