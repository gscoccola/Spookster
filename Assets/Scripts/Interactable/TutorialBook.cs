using TMPro;
using UnityEngine;

public class TutorialBook : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    [SerializeField] TextMeshPro text1;
    [SerializeField] TextMeshPro text2;
    [SerializeField] TextMeshPro text3;
    [SerializeField] TextMeshPro text4;

    bool wasInitialized;

    private void Awake()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        playerShooting = FindAnyObjectByType<PlayerShooting>();
    }

    private void Update()
    {
        switch (playerMovement.tutorialStage)
        {
            case 1:
                text1.enabled = true;
                wasInitialized = true;
                break;
            case 2:
                text2.enabled = true;
                break;
            case 3:
                text3.enabled = true;
                break;
            case 4:
                text4.enabled = true;
                break;
            case 0:
                if (!wasInitialized)
                {
                    text1.enabled = false;
                    text2.enabled = false;
                    text3.enabled = false;
                    text4.enabled = false;
                }
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
           StartTutorial();
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
           // EndLoadingTutorial();
        }
    }


    public void StartTutorial()
    {
        if (!wasInitialized)
        {
            playerMovement.SetTutorialStage(1);
        }
    }
}
