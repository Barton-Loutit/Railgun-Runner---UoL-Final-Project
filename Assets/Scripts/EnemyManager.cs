/*
 * This script is responsible for handling enemy death, effects,
 * and collisions with player weapons.
 * Also responsible for managing enemy statistics including hit points,
 * hit score, and death score. 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] GameObject enemyHitVFX;
    [SerializeField] GameObject enemyDeathVFX;
    [SerializeField] int enemyHitScoreValue = 0;
    [SerializeField] int enemyDeathScoreValue = 0;
    [SerializeField] int enemyHitPoints = 3;

    //Process enemy hit on particle collision iff collision source is a
    //"PlayerWeapon" Other particles, such as other enemy missiles in the
    //future could trigger this otherwise.
    void OnParticleCollision(GameObject other)
    {
        if(other.tag == "PlayerWeapon")
        {
            HandleEnemyHit(1);
        }
    }

    //This function manages collisions with the player's bomb, and has
    //functionality to manage collision with other weapons in the
    //future that aren't particle based (hence the inclusion of 
    //both collision and collider tags).
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with" + collision.gameObject.name);
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            HandleEnemyHit(50);
        }
        if(collision.collider.gameObject.tag == "PlayerWeapon")
        {
            HandleEnemyHit(50);
        }
    }

    //When the enemy is hit, take damage. If that damage reduces them
    //below 0 HP, they die. Play VFX, SFX, handle score, and destroy
    //the enemy game object.
    //If the damage does not kill them, just play a (different, less major)
    // VFX and SFX, and increment the score according to enemyHitScore.
    void HandleEnemyHit(int damage)
    {
        enemyHitPoints -= damage;
        if(enemyHitPoints <= 0)
        {
            HandleEnemyDeath();
        }
        else
        {
            PlayerStatManager.playerStatManagerInstance.updateScore(enemyHitScoreValue);
            AudioManager.audioManagerInstance.Play("EnemyHitSFX");
            PlayEnemyVFX(enemyHitVFX);
        }
    }

    //As above - when HP <= 0, play VFX, SFX, update score, and destroy the enemy game
    //object.
    void HandleEnemyDeath()
    {
        PlayEnemyVFX(enemyDeathVFX);
        AudioManager.audioManagerInstance.Play("EnemyDeathSFX");
        PlayerStatManager.playerStatManagerInstance.updateScore(enemyDeathScoreValue);
        DestroyEnemyObject();
    }

    //Logic to destroy the game object. Resistant to multiple simultaneous calls.
    void DestroyEnemyObject()
    {
        if (!this.gameObject.IsDestroyed())
        {
            Destroy(this.gameObject);
        }
    }

    //When a VFX game object is instantiated, it will exist until destroyed.
    //Each VFX is assigned a "Destroy when X time has passed" script to remove
    //the instantiated object when appropriate.
    void PlayEnemyVFX(GameObject enemyVFXToPlay)
    {
        GameObject vfx = Instantiate(enemyVFXToPlay, transform.position, Quaternion.identity);
    }

    //Plays the relevant SFX when the enemy is hit.
    void PlayEnemyHitSFX()
    {
        AudioManager.audioManagerInstance.Play("EnemyHitSFX");
    }   
}
