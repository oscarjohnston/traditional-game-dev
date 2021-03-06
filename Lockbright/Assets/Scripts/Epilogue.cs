﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Epilogue : MonoBehaviour
{
    private bool ButtonIsPressable;
    private bool OnCreditScreen = false;

    public GameObject CreditScreen;

    public Image AButton;

    // Start is called before the first frame update
    void Start()
    {
        ButtonIsPressable = false;
        AButton.gameObject.SetActive(false);
        Invoke("MakeButtonPressable", 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ButtonIsPressable) { return; }

        if (Input.GetButtonDown("A_Button_1") || Input.GetButtonDown("A_Button_2") || Input.GetButtonDown("A_Button_3") || Input.GetButtonDown("A_Button_4"))
        {
            if (OnCreditScreen)
            {
                GoBackToMainMenu();
            }
            else
            {
                ButtonIsPressable = false;
                AButton.gameObject.SetActive(false);
                MoveToCredits();
                Invoke("MakeButtonPressable", 2.0f);
            }
        }
    }

    private void MoveToCredits()
    {
        OnCreditScreen = true;
        CreditScreen.SetActive(true);
    }

    private void GoBackToMainMenu()
    {
        // Go to main game scene
        SceneManager.LoadScene("Menu");
    }

    private void MakeButtonPressable()
    {
        ButtonIsPressable = true;
        AButton.gameObject.SetActive(true);
    }
}
