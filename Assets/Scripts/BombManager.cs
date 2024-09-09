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
    [SerializeField] int damage = 50;
    [SerializeField] Quaternion shipRotation;
    Vector3 normalizedPosition;
    // Start is called before the first frame update
    void Start()
    {
        //playerController = FindObjectOfType<PlayerController>();
    }

    void OnEnable()
    {
     
        this.gameObject.transform.rotation = playerShip.transform.rotation;

        this.gameObject.transform.position = playerShip.transform.position+ (transform.position.normalized + (40 * transform.forward));
             
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
