using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Can add pickup VFX, SFX if we want
public class PickupHandler : MonoBehaviour
{
    [SerializeField] int numPickupsToAdd = 1;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {

            PlayerStatManager.playerStatManagerInstance.updateBombs(numPickupsToAdd);
            Destroy(this.gameObject);
        }
    }
}
