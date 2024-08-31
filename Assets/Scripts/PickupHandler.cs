using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Can add pickup VFX, SFX if we want
public class PickupHandler : MonoBehaviour
{
    PlayerStatManager playerStatManager;
    [SerializeField] int numPickupsToAdd = 1;

    // Start is called before the first frame update
    void Start()
    {
        playerStatManager = FindObjectOfType<PlayerStatManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            playerStatManager.updateBombs(numPickupsToAdd);
            Destroy(this.gameObject);
        }
    }
}
