using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] Canvas pauseScreen;
    [SerializeField] Button retryButton;
    [SerializeField] Button quitButton;

    [SerializeField] Image timerOptionCross;
    [SerializeField] Image slowmoOptionCross;

    AudioPlayer audioPlayer;
    bool[] levelCompletion;

    int sceneIndex;

    public bool isSlowMotionEnabled;
    bool isSlowMotionActivated;
    [SerializeField] const float slowMotionDuration = 3f;
    [SerializeField] const float slowMotionPercentage = 0.5f;

    public bool isTimerDisplayed;

    public bool isTimerEnabled;
    double levelTimer;

    Coroutine currentSlowMoCoroutine;

    void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
        retryButton.onClick.AddListener(ReloadLevel);
        quitButton.onClick.AddListener(ExitLevel);
        ManageSingleton();
    }

    private void Start()
    {
        audioPlayer.SwitchTrack();
        levelCompletion = new bool[10];
    }

    private void Update()
    {
        HandleLevelTimer();
    }

    void ManageSingleton()
    {
        int numGameManagers = FindObjectsByType<GameManager>(FindObjectsSortMode.None).Length;
        if (numGameManagers > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            UnPauseGame();
        }
    }

    void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseScreen.enabled = true;
        AudioListener.pause = true;
    }

    void UnPauseGame()
    {
        isPaused = false;
        Time.timeScale = isSlowMotionActivated ? slowMotionPercentage : 1;
        pauseScreen.enabled = false;
        AudioListener.pause = false;
    }

    public IEnumerator ReloadSceneCoroutine()
    {
        yield return new WaitForSeconds(2f);
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevel(currentSceneIndex);
    }

    void ExitLevel()
    {
        LoadLevel(0);
    }

    public void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        LoadLevel(nextSceneIndex);
    }

    public void LoadLevel(int index)
    {
        UnPauseGame();
        if (index != SceneManager.GetActiveScene().buildIndex)
        {
            audioPlayer.SwitchTrack();
        }
        SceneManager.LoadScene(index);
        sceneIndex = index;
        levelTimer = 0.0;
        isSlowMotionActivated = false;
    }

    public double GetLevelTime() => levelTimer;

    public void ToggleLevelTimer()
    {
        isTimerEnabled = !isTimerEnabled;
        timerOptionCross.enabled = isTimerEnabled;
    }

    public void ToggleSlowMotionOption()
    {
        isSlowMotionEnabled = !isSlowMotionEnabled;
        slowmoOptionCross.enabled = isSlowMotionEnabled;
    }

    void HandleLevelTimer()
    {
        if (!isPaused) { levelTimer += Time.deltaTime;}
        if (sceneIndex != 0 && isTimerEnabled)
        {
            isTimerDisplayed = true;
            return;
        }
        isTimerDisplayed = false;
    }

    public void StartSlowMotion()
    {
        if (!isSlowMotionEnabled) { return; }
        isSlowMotionActivated = true;
        Time.timeScale = slowMotionPercentage;
        currentSlowMoCoroutine = StartCoroutine(SlowMotionCoroutine());
    }

    public void StopSlowMotion()
    {
        if (currentSlowMoCoroutine is not null)
        {
            StopCoroutine(currentSlowMoCoroutine);
            Time.timeScale = 1;
            isSlowMotionActivated = false;
        }
    }

    public IEnumerator SlowMotionCoroutine()
    {
        yield return new WaitForSecondsRealtime(slowMotionDuration);
        Time.timeScale = 1;
        isSlowMotionActivated = false;
    }
}
