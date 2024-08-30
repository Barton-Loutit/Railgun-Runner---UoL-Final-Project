using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombManager : MonoBehaviour
{
    [SerializeField] ParticleSystem bombParticleSystem;
    [SerializeField] float psTotalLifetime;
    [SerializeField] GameObject playerShip;
    [SerializeField] Vector3 shipOffset;
    // Start is called before the first frame update
    void Start()
    {
        //playerController = FindObjectOfType<PlayerController>();
    }

    void OnEnable()
    {
        this.gameObject.transform.position = playerShip.transform.position + shipOffset;
        bombParticleSystem.Play();
        Invoke("DisableSelf", psTotalLifetime);
    }

    void DisableSelf()
    {
        //this.gameObject.transform.position = this.gameObject.transform.parent.position + new Vector3(0,0,40);
        this.gameObject.SetActive(false);
        
    }


}
