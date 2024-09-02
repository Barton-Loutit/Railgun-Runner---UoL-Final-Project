using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BombManager : MonoBehaviour
{
    [SerializeField] ParticleSystem bombParticleSystem;
    [SerializeField] float psTotalLifetime;
    [SerializeField] GameObject playerShip;
    /*[SerializeField] Vector3 shipOffset;*/
    [SerializeField] float bombActivationDistance = 40f;
    [SerializeField] Quaternion shipRotation;
    Vector3 normalizedPosition;
    // Start is called before the first frame update
    void Start()
    {
        //playerController = FindObjectOfType<PlayerController>();
    }

    void OnEnable()
    {
        /*this.gameObject.transform.position = playerShip.transform.position;*/
        this.gameObject.transform.rotation = playerShip.transform.rotation;

        this.gameObject.transform.position = playerShip.transform.position+ (transform.position.normalized + (40 * transform.forward));


        /*shipRotation = playerShip.transform.localRotation;
        normalizedPosition = playerShip.transform.localPosition.normalized;
        *//*shipOffset = playerShip.transform.GetLocalPositionAndRotation();*//*
        this.gameObject.transform.rotation = shipRotation;
        this.gameObject.transform.position = playerShip.transform.position;

        this.gameObject.transform.position += bombActivationDistance * normalizedPosition;*/
        /*this.gameObject.transform.position = playerShip.transform.position + (bombActivationDistance * this.gameObject.transform.forward);*/

        /*this.gameObject.transform.position = playerShip.transform.position + shipOffset;*/
        bombParticleSystem.Play();
        Invoke("DisableSelf", psTotalLifetime);
    }

    void DisableSelf()
    {
        //this.gameObject.transform.position = this.gameObject.transform.parent.position + new Vector3(0,0,40);
        this.gameObject.SetActive(false);
        
    }

    void OnDrawGizmos()
    {
            
    }

}
