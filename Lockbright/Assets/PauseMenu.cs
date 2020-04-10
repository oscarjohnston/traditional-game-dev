using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject PauseMenuUI;

    // This variable is so only one player can control the pause screen at one time
    private int PlayerWhoPaused = 1;

    public Button ResumeButton;
    public Button HighlightedButton;

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

        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        EventSystem.current.SetSelectedGameObject(this.gameObject);
        EventSystem.current.SetSelectedGameObject(ResumeButton.gameObject);
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

    /*
    /// <summary>
    /// Sends an input to the party menu.
    /// </summary>
    public void SendPartyMenuInput(MenuInputs menuInput)
    {
        switch (menuInput)
        {
            case (MenuInputs.Up):
                if (highlightedButton.navigation.selectOnUp != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.navigation.selectOnUp.gameObject);
                    highlightedButton = highlightedButton.navigation.selectOnUp.GetComponent<Button>();
                }
                else if (highlightedButton.FindSelectableOnUp() != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.FindSelectableOnUp().gameObject);
                    highlightedButton = highlightedButton.FindSelectableOnUp().GetComponent<Button>();
                }
                break;
            case (MenuInputs.Right):
                if (highlightedButton.navigation.selectOnRight != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.navigation.selectOnRight.gameObject);
                    highlightedButton = highlightedButton.navigation.selectOnRight.GetComponent<Button>();
                }
                else if (highlightedButton.FindSelectableOnRight() != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.FindSelectableOnRight().gameObject);
                    highlightedButton = highlightedButton.FindSelectableOnRight().GetComponent<Button>();
                }
                break;
            case (MenuInputs.Left):
                if (highlightedButton.navigation.selectOnLeft != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.navigation.selectOnLeft.gameObject);
                    highlightedButton = highlightedButton.navigation.selectOnLeft.GetComponent<Button>();
                }
                else if (highlightedButton.FindSelectableOnLeft() != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.FindSelectableOnLeft().gameObject);
                    highlightedButton = highlightedButton.FindSelectableOnLeft().GetComponent<Button>();
                }
                break;
            case (MenuInputs.Down):
                if (highlightedButton.navigation.selectOnDown != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.navigation.selectOnDown.gameObject);
                    highlightedButton = highlightedButton.navigation.selectOnDown.GetComponent<Button>();
                }
                else if (highlightedButton.FindSelectableOnDown() != null)
                {
                    EventSystem.current.SetSelectedGameObject(highlightedButton.FindSelectableOnDown().gameObject);
                    highlightedButton = highlightedButton.FindSelectableOnDown().GetComponent<Button>();
                }
                break;
            case (MenuInputs.Confirm):
                if (highlightedButton != null)
                    highlightedButton.onClick.Invoke();
                break;
        }

        switch (currentPartyMenuScreen)
        {
            case (PartyMenuScreens.Main):
                MainScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.GeneralItems):
                GeneralItemsScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.BattleItems):
                BattleItemsScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.Accessories):
                AccessoriesScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.Provisions):
                ProvisionsScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.Status):
                StatusScreen.HandleScreenInputs(menuInput);
                break;
            case (PartyMenuScreens.Controls):
                ControlsScreen.HandleScreenInputs(menuInput);
                break;
                //case(PartyMenuScreens.Options):
                //    OptionsScreen.HandleScreenInputs(menuInput);
                //    break;
        }
    }*/

}


