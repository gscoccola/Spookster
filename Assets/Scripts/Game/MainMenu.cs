using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Unity.Mathematics;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] float outerLayerMovementRatio = 6f;
    [SerializeField] float innerLayerMovementRatio = 4f;
    [SerializeField] float textLayerMovementRatio = 10f;
    [SerializeField] float outerVibrationAmplitude = 0.1f;
    [SerializeField] float innerVibrationAmplitude = 0.1f;
    [SerializeField] float twitchPeriod = 2.5f;
    [SerializeField] float twitchDuration = 0.1f;

    [Header("Parallax images")]
    [SerializeField] RawImage outerImage;
    [SerializeField] RawImage innerImage;

    [Header("Main Menu")]
    [SerializeField] GameObject mainMenu;
    [SerializeField] Button levelSelectButton;
    [SerializeField] Button optionsButton;

    [Header("Level Select")]
    [SerializeField] GameObject levelSelectMenu;
    [SerializeField] List<Button> levelButtonList;
    [SerializeField] List<Button> hardLevelButtonList;
    [SerializeField] Button backButton;

    [Header("Options")]
    [SerializeField] GameObject optionsMenu;
    [SerializeField] Button backButton2;

    Vector3 startingPos;
    System.Random random;
    int framesPassed;
    Vector3 randomVectorInner, randomVectorOuter;
    bool isTwitching;



    private void Awake()
    {
        startingPos = outerImage.transform.position;
        levelSelectButton.onClick.AddListener(DisplayLevels);
        optionsButton.onClick.AddListener(DisplayOptions);
        backButton.onClick.AddListener(ReturnFromLevelSelect);
        backButton2.onClick.AddListener(ReturnFromOptions);


        /*for (int i = 0; i < levelButtonList.Count; i++)
        {
            levelButtonList[i].onClick.AddListener(() => LoadLevel(i));
        }*/
        levelButtonList[0].onClick.AddListener(() => LoadLevel(1));
        levelButtonList[1].onClick.AddListener(() => LoadLevel(2));
        levelButtonList[2].onClick.AddListener(() => LoadLevel(3));
        levelButtonList[3].onClick.AddListener(() => LoadLevel(4));
        levelButtonList[4].onClick.AddListener(() => LoadLevel(5));
        levelButtonList[5].onClick.AddListener(() => LoadLevel(6));
        levelButtonList[6].onClick.AddListener(() => LoadLevel(7));
        levelButtonList[7].onClick.AddListener(() => LoadLevel(8));
        hardLevelButtonList[0].onClick.AddListener(() => LoadLevel(9));
        hardLevelButtonList[1].onClick.AddListener(() => LoadLevel(10));
    }

    private void Start()
    {
        random = new System.Random();
        StartCoroutine(ITwitching());
    }

    void Update()
    {
        if (isTwitching)
        {
            float randomAngleInner = (float)random.NextDouble() * 2 * MathF.PI;
            float randomAngleOuter = (float)random.NextDouble() * 2 * MathF.PI;
            randomVectorInner = new Vector3(Mathf.Cos(randomAngleInner), Mathf.Sin(randomAngleInner), 0f) * innerVibrationAmplitude;
            randomVectorOuter = new Vector3(Mathf.Cos(randomAngleOuter), Mathf.Sin(randomAngleOuter), 0f) * outerVibrationAmplitude;
        }

        this.transform.position = startingPos - (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / textLayerMovementRatio;

        outerImage.transform.position = startingPos + randomVectorOuter +
            (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / outerLayerMovementRatio;

        innerImage.transform.position = startingPos + randomVectorInner +
            (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / innerLayerMovementRatio;
    }

    IEnumerator ITwitching()
    {
        yield return new WaitForSecondsRealtime(twitchPeriod - twitchDuration);
        isTwitching = true;
        AudioPlayer.Instance.PlayHearbeatClip();
        yield return new WaitForSecondsRealtime(twitchDuration);
        isTwitching = false;
        StartCoroutine(ITwitching());
    }

    void DisplayLevels()
    {
        AudioPlayer.Instance.PlayReloadingClip();
        levelSelectMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void ReturnFromLevelSelect()
    {
        AudioPlayer.Instance.PlayDryFireClip();
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void DisplayOptions()
    {
        AudioPlayer.Instance.PlayReloadingClip();
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void ReturnFromOptions()
    {
        AudioPlayer.Instance.PlayDryFireClip();
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void LoadLevel(int index)
    {
        AudioPlayer.Instance.PlayShootingClip1(true);
        GameManager.Instance.LoadLevel(index);
    }

}
