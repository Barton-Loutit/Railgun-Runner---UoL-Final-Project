using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//I'm not thrilled with the implementation of Timer, TimerText, and ContinueText Managers.
//Ultimately this would make sense to be managed at the canvas level via serialized fields, but this
//hacky implementation gets the job done.
public class ContinueTextManager : MonoBehaviour
{
    TMP_Text continueText;
    Timer countdownTimer;
    TextMeshProUGUI textMeshProUGUI;

    // Start is called before the first frame update
    void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.enabled = false;
    }

    public void SetTextRendering(bool state)
    {
        textMeshProUGUI.enabled = state;
    }
}
