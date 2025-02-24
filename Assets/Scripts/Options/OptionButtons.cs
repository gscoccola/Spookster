using UnityEngine;
using UnityEngine.UI;

public class OptionButtons : MonoBehaviour
{
    [SerializeField] Button levelTimerButton;
    [SerializeField] Button slowMotionButton;
    GameManager gameManager;
    Image timerButtonCross;
    Image slowMotionButtonCross;

    private void Awake()
    {
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
        gameManager.ToggleLevelTimer();
        timerButtonCross.enabled = gameManager.isTimerEnabled;
    }

    void ToggleSlowMotion()
    {
        gameManager.ToggleSlowMotionOption();
        slowMotionButtonCross.enabled = gameManager.isSlowMotionEnabled;
    }
}
