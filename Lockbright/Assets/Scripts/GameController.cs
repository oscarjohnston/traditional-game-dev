﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Sound Effects for game
    public UnityEvent StartBackgroundMusic;
    public UnityEvent ParkouristAbilitySound;
    public UnityEvent BurnerAbilitySound;
    public UnityEvent ScholarAbilitySound;
    public UnityEvent IllusionistAbilitySound;
    public UnityEvent TakeDamageSound;

    public float TimeLeft;
    public Text TimerText;
    public Text GameOverText;

    // Bookshelf Puzzle
    private int BookshelfCounter = 3;

    // Start is called before the first frame update
    void Start()
    {
        // On start, play the background music
        StartBackgroundMusic.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if(TimeLeft < 0)
        {
            GameOverText.gameObject.SetActive(true);
            Invoke("SendToMainMenu", 3f);
        }
        else
        {
            TimerText.text = "" + ((int)TimeLeft / 60) + ":" + ((int)TimeLeft % 60);
        }
    }

    void SendToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }



    public void PlayParkouristAbilitySound()
    {
        ParkouristAbilitySound.Invoke();
    }

    public void PlayBurnerAbilitySound()
    {
        BurnerAbilitySound.Invoke();
    }

    public void PlayScholarAbilitySound()
    {
        ScholarAbilitySound.Invoke();
    }

    public void PlayIllusionistAbilitySound()
    {
        IllusionistAbilitySound.Invoke();
    }

    /// <summary>
    /// This event will win the game
    /// </summary>
    public void InvokeWinGameEvent()
    {
        // Go to main game scene
        SceneManager.LoadScene("Epilogue");
    }

    public void PlayTakeDamageSound()
    {
        TakeDamageSound.Invoke();
    }

    public void DecrementBookshelfCounter()
    {
        BookshelfCounter--;

        // If the requirements fulfilled, spawn the Dusty Record
        if(BookshelfCounter == 0)
        {
            // TODO: Spawn Dusty Record
        }
    }
}
