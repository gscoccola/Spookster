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
        gameManager = FindAnyObjectByType<GameManager>();
        levelTimer = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        levelTimer.text = Math.Round(gameManager.GetLevelTime(), 2).ToString();
        if (gameManager.isTimerDisplayed)
        {
            levelTimer.enabled = true;
            return;
        }
        levelTimer.enabled = false;
    }
}
