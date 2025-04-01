using UnityEngine;
using UnityEngine.UI;

public class OptionButtons : MonoBehaviour
{
    [SerializeField] Button levelTimerButton;
    [SerializeField] Button slowMotionButton;
    Image timerButtonCross;
    Image slowMotionButtonCross;

    private void Awake()
    {
        timerButtonCross = levelTimerButton.transform.GetChild(1).GetComponent<Image>();
        slowMotionButtonCross = slowMotionButton.transform.GetChild(1).GetComponent<Image>();
        levelTimerButton.onClick.AddListener(ToggleTimer);
        slowMotionButton.onClick.AddListener(ToggleSlowMotion);
    }

    void Start()
    {

        //SetButtons();
        timerButtonCross.enabled = GameManager.Instance.isTimerEnabled;
        slowMotionButtonCross.enabled = GameManager.Instance.isSlowMotionEnabled;

        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleTimer()
    {
        AudioPlayer.Instance.PlayDryFireClip();
        GameManager.Instance.ToggleLevelTimer();
        timerButtonCross.enabled = GameManager.Instance.isTimerEnabled;
    }

    void ToggleSlowMotion()
    {
        AudioPlayer.Instance.PlayDryFireClip();
        GameManager.Instance.ToggleSlowMotionOption();
        slowMotionButtonCross.enabled = GameManager.Instance.isSlowMotionEnabled;
    }
}
