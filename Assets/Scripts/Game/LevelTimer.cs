using System;
using TMPro;
using UnityEngine;

public class LevelTimer : MonoBehaviour
{

    TextMeshProUGUI levelTimer;
    double timer;
    GameManager gameManager;

    private void Awake()
    {
        levelTimer = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        levelTimer.text = Math.Round(GameManager.Instance.GetLevelTime(), 2).ToString();
        if (GameManager.Instance.isTimerDisplayed)
        {
            levelTimer.enabled = true;
            return;
        }
        levelTimer.enabled = false;
    }
}
