using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerTextManager : MonoBehaviour
{
    TMP_Text timerText;
    Timer countdownTimer;
    TextMeshProUGUI textMeshProUGUI;
    ContinueTextManager continueTextManager;

    // Start is called before the first frame update
    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        countdownTimer = FindObjectOfType<Timer>();
        continueTextManager = FindObjectOfType<ContinueTextManager>();

        timerText.text = countdownTimer.getTimerValue().ToString();

        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        textMeshProUGUI.enabled = false;
    }

    public void UpdateTimerText()
    {
        timerText.text = countdownTimer.getTimerValue().ToString();
    }

    public void SetTextRendering(bool state)
    {
        textMeshProUGUI.enabled = state;
        continueTextManager.SetTextRendering(state);
    }
}
