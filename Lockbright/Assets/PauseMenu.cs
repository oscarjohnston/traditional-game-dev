using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    // This variable is so only one player can control the pause screen at one time
    private int PlayerWhoPaused;

    // Update is called once per frame
    void Update()
    {
        // This section is for an unpaused game
        if (!GameIsPaused)
        {
            // Check if it's time to pause the game
            if (StartButtonIsPressedToPause() || Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
                return;
            }
        }

        // This section is for a paused game
        else
        {
            // Check if it's time to play the game again by the player who paused
            if (Input.GetButtonDown("Start_Button_" + PlayerWhoPaused) || Input.GetKeyDown(KeyCode.Escape))
            {
                Resume();
                return;
            }
        }
        
    }
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Helper method for checking if any of the controllers pressed the start button, then recording who did
    /// </summary>
    /// <returns></returns>
    private bool StartButtonIsPressedToPause()
    {
        if (Input.GetButtonDown("Start_Button_1"))
        {
            PlayerWhoPaused = 1;
            return true;
        }
        else if (Input.GetButtonDown("Start_Button_2"))
        {
            PlayerWhoPaused = 2;
            return true;
        }
        else if (Input.GetButtonDown("Start_Button_3"))
        {
            PlayerWhoPaused = 3;
            return true;
        }
        else if (Input.GetButtonDown("Start_Button_4"))
        {
            PlayerWhoPaused = 4;
            return true;
        }
        else { return false; }
    }

}
