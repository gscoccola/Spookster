using UnityEngine;
using UnityEngine.UI;

public class OptionButtons : MonoBehaviour
{
    [SerializeField] Button levelTimerButton;
    [SerializeField] Button slowMotionButton;
    GameManager gameManager;
    Image timerButtonCross;
    Image slowMotionButtonCross;
    AudioPlayer audioPlayer;

    private void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
        gameManager = FindAnyObjectByType<GameManager>();
        timerButtonCross = levelTimerButton.transform.GetChild(1).GetComponent<Image>();
        slowMotionButtonCross = slowMotionButton.transform.GetChild(1).GetComponent<Image>();
        levelTimerButton.onClick.AddListener(ToggleTimer);
        slowMotionButton.onClick.AddListener(ToggleSlowMotion);
    }

    void Start()
    {

        //SetButtons();
        timerButtonCross.enabled = gameManager.isTimerEnabled;
        slowMotionButtonCross.enabled = gameManager.isSlowMotionEnabled;

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleTimer()
    {
        audioPlayer.PlayDryFireClip();
        gameManager.ToggleLevelTimer();
        timerButtonCross.enabled = gameManager.isTimerEnabled;
    }

    void ToggleSlowMotion()
    {
        audioPlayer.PlayDryFireClip();
        gameManager.ToggleSlowMotionOption();
        slowMotionButtonCross.enabled = gameManager.isSlowMotionEnabled;
    }
}
