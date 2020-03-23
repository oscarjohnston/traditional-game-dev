﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    // Controller for particle
    public UIController game;

    // Boolean for identifying if the stove is on
    private bool StoveIsLit;

    // Required Objects
    public GameObject PaleTonic;
    public GameObject LivingMossFlower;
    public GameObject Pomegranate;

    // Number of items still required
    public int itemsleft = 3;

    // Interaction helpers
    public GameObject reward;
    public bool spawned;
    public string RewardText;
    public string InteractionText;

    void Start()
    {
        StoveIsLit = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Try to turn the stove on if it's not lit yet
        if (!StoveIsLit)
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            // If the burner presses Y, light the stove
            if (player != null && Input.GetButtonDown("Y_Button_3") && player.PlayerNumber == 3)
            {
                StoveIsLit = true;
                game.FireTurnOnStove();

                print("Turning on stove");
            }
        }

        // Otherwise turn it off
        else
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            // If the burner presses Y, light the stove
            if (player != null && Input.GetButtonDown("Y_Button_3") && player.PlayerNumber == 3)
            {
                StoveIsLit = false;
                game.FireTurnOffStove();

                print("Turning off stove");
            }
        }
    }
    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        // No items added gives first message
        if (itemsleft == 3)
        {
            BubbleText.text = InteractionText;
        }
        // Otherwise gives this message
        else
        {
            BubbleText.text = "Seems like we're missing something...";
        }

        // Check if this object has spawned its item yet
        if (!spawned)
        {
            // Is the player holding one of the right items? (and optionally the correct Player?)
            if (HeldItem == PaleTonic)
            {
                itemsleft--;

                BubbleText.text = "Good job! You only need " + itemsleft + "more items it seems";

                Destroy(HeldItem);

                grabbed = false;
            }
            if (HeldItem == Pomegranate)
            {
                itemsleft--;

                BubbleText.text = "Good job! You only need " + itemsleft + "more items it seems";

                Destroy(HeldItem);

                grabbed = false;
            }
            if (HeldItem == LivingMossFlower)
            {
                itemsleft--;

                BubbleText.text = "Good job! You only need " + itemsleft + "more items it seems";

                Destroy(HeldItem);

                grabbed = false;
            }


            if (reward != null && itemsleft == 0 && StoveIsLit == true)
            {
                HeldItem = reward;
                grabbed = true;
                BubbleText.text = RewardText;
            }
            else
            {
                grabbed = false;
            }
            //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
            spawned = true;
        }
    }
}
