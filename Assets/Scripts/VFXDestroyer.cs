using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Creating this class, which will get thrown on my VFX to destroy the 
// instantiated game object after the specified delay.
public class VFXDestroyer : MonoBehaviour
{
    [SerializeField] float destructionDelay = 1f;

    // Start is called before the first frame update
    void Start()
    {
        if (!this.gameObject.IsDestroyed())
        {
            Destroy(this.gameObject, destructionDelay);
        }
    }
}
