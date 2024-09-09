/*
 * This class manages the player's bomb lifetime.
 * EFfectively, when the bomb is enabled, update the position to a set
 * distance ahead of the player (40 units), and enable the bomb. On
 * enable, the bomb should activate its VFX (SFX is managed in the Player
 * Controller script), and subsequently disable itself (to allow for 
 * multiple bomb uses) after the particle has finished playing.
 * (as defined by the dev in psTotalLifetime).
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class BombManager : MonoBehaviour
{
    [SerializeField] ParticleSystem bombParticleSystem;
    [SerializeField] float psTotalLifetime;
    [SerializeField] GameObject playerShip;
    [SerializeField] float bombActivationDistance = 40f;
    [SerializeField] int damage = 50;
    [SerializeField] Quaternion shipRotation;
    Vector3 normalizedPosition;

    //Copy the rotation of the player's ship, then update the position
    //of the bomb to bombActivationDistance units ahead of the direction
    //the player's ship is facing.
    void OnEnable()
    {
     
        this.gameObject.transform.rotation = playerShip.transform.rotation;

        this.gameObject.transform.position = playerShip.transform.position+ (transform.position.normalized + (40 * transform.forward));
             
        bombParticleSystem.Play();
        Invoke("DisableSelf", psTotalLifetime);
    }

    void DisableSelf()
    {
        this.gameObject.SetActive(false);
        
    }

}
