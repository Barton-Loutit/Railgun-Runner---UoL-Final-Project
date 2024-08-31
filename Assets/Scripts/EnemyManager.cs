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

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with" + collision.gameObject.name);
        if (collision.gameObject.tag == "PlayerWeapon")
        {
            HandleEnemyHit();
        }
        if(collision.collider.gameObject.tag == "PlayerWeapon")
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
            PlayerStatManager.playerStatManagerinstance.updateScore(enemyHitScoreValue);
            PlayEnemyVFX(enemyHitVFX);

            //To-Do: Add SFX for enemy hit (Note-to-self: use serialized field & setup an audio manager singleton)
        }
    }
    void HandleEnemyDeath()
    {
        PlayEnemyVFX(enemyDeathVFX);
        PlayerStatManager.playerStatManagerinstance.updateScore(enemyDeathScoreValue);
        //scoreManager.UpdateScore(enemyDeathScoreValue);
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
