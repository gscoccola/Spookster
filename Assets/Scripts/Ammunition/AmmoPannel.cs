using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class AmmoPannel : MonoBehaviour
{
    [SerializeField] List<GameObject> shellSlots = new List<GameObject>();
    AudioPlayer audioPlayer;

    private void Awake()
    {
        audioPlayer = FindAnyObjectByType<AudioPlayer>();
    }

    void SetShellType(int i, int shellType)
    {
        shellSlots[i].GetComponent<Shell>().OnShellPickup(shellType);
    }

    public bool OnShellPickup(int pickedUpType)
    {
        int currentShellType = -1;
        int currentIndex = -1;

        while (currentShellType != 0)
        {
            //Debug.Log(currentIndex + " is type " + currentShellType);
            currentIndex++;
            if (currentIndex >= shellSlots.Count) { return false; } 
            currentShellType = shellSlots[currentIndex].GetComponent<Shell>().GetShellType();
        }
        shellSlots[currentIndex].GetComponent<Shell>().OnShellPickup(pickedUpType);
        audioPlayer.PlayPickupClip();
        return true;
        //Debug.Log(currentIndex + " has been set to " + pickedUpType);
    }

}
