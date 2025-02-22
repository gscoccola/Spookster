using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelExit : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 3f;
    [SerializeField] GameObject sliderGO;
    Slider slider;
    bool isPlayerInside;
    AudioSource audioSource;
    GameManager gameManager;
    bool hasFoundManager;

    private void Start()
    {
        slider = sliderGO.GetComponent<Slider>();
        slider.maxValue = levelLoadDelay;
        audioSource = GetComponent<AudioSource>();
    }

 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isPlayerInside = true;
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            isPlayerInside = false;
            
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void Update()
    {
        if (!hasFoundManager)
        {
            hasFoundManager = true;
            gameManager = FindAnyObjectByType<GameManager>();
        }

        if (!isPlayerInside)
        {
            sliderGO.SetActive(false);
            slider.value = slider.maxValue;
            
            
        }
        else
        {
            sliderGO.SetActive(true);
            slider.value -= Time.deltaTime;
            
            if (slider.value == 0)
            {
                gameManager.LoadNextLevel();
            }
        }
    }

    

}

