using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] Canvas pauseScreen;
    [SerializeField] Button retryButton;
    [SerializeField] Button quitButton;
    AudioPlayer audioPlayer;
    bool[] levelCompletion;

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
        Time.timeScale = 1;
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
    }
}
