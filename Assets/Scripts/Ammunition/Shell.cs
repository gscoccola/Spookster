using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Shell : MonoBehaviour, IPointerClickHandler
{
    PlayerShooting playerShooting;
    int shellType = 0;
    RawImage shellImage;
    Button button;
    [Header("Sprites")]
    [SerializeField] List<Texture> shellSprites = new List<Texture>();

    void Awake()
    {
        playerShooting = FindAnyObjectByType<PlayerShooting>();
        button = GetComponent<Button>();
        shellImage = GetComponent<RawImage>();
    }

    void Start()
    {
        DisplayShell();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
        if (!playerShooting.inMenu) { return; }
        if (!playerShooting.isAlive) { return; }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            int alreadyLoadedShell = playerShooting.GetLoadedShell(0);
            shellType = playerShooting.ReloadBarrel(0, shellType);
            DisplayShell();
        }
            
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            int alreadyLoadedShell = playerShooting.GetLoadedShell(1);
            shellType = playerShooting.ReloadBarrel(1, shellType);
            DisplayShell();
        }
        
 
        
    }

    void DisplayShell()
    {
        if (shellType == 0)
        {
            button.interactable = false;
        }
        else
        {
            shellImage.texture = shellSprites[shellType - 1];
            button.interactable = true;
        }
    }

    public int GetShellType()
    {
        return shellType;
    }

    public void OnShellPickup(int type)
    {
        shellType = type;
        DisplayShell();
    }
}