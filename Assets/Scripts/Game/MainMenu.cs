using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Parallax images")]
    [SerializeField] RawImage outerImage;
    [SerializeField] RawImage innerImage;
    [SerializeField] float outerLayerMovementRatio = 6f;
    [SerializeField] float innerLayerMovementRatio = 4f;


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

    AudioPlayer audioPlayer;
    Vector3 startingPos;
    GameManager gameManager;

    private void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
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

    void Update()
    {
        this.transform.position = startingPos + (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / 20;
        outerImage.transform.position = startingPos + (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / outerLayerMovementRatio;
        innerImage.transform.position = startingPos + (Input.mousePosition - new Vector3(Screen.width / 2, Screen.height / 2, 0)) / innerLayerMovementRatio;
    }

    void DisplayLevels()
    {
        audioPlayer.PlayReloadingClip();
        levelSelectMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void ReturnFromLevelSelect()
    {
        audioPlayer.PlayDryFireClip();
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void DisplayOptions()
    {
        audioPlayer.PlayReloadingClip();
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    void ReturnFromOptions()
    {
        audioPlayer.PlayDryFireClip();
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    void LoadLevel(int index)
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioPlayer.PlayShootingClip1(true);
        gameManager.LoadLevel(index);
    }

}
