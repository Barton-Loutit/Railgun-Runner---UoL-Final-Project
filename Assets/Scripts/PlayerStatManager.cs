using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStatManager : MonoBehaviour
{
    TMP_Text bombsText;

    //[SerializeField] int health;
    int bombCount = 0;

    void Start()
    {
        bombsText = GetComponent<TMP_Text>();
        bombsText.text = "Bombs: " + getBombCount().ToString();
    }
    public void updateBombs(int numBombsToPickup)
    {
        bombCount += numBombsToPickup;
        bombsText.text = "Bombs: " + getBombCount().ToString();
    }

    public int getBombCount()
    {
        return bombCount; 
    }
}
