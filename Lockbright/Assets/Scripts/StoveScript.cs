using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    // Controller for particle
    public UIController game;

    // Bollean for identifying if the stove is on
    private bool StoveIsLit;

    // Required Items
    public GameObject Pomegranate;
    public GameObject PaleTonic;
    public GameObject LivingMossFlower;

    // Number of items left
    public int itemsLeft = 3;

    // Reward
    public GameObject CultistWine;

    public bool spawned;
    public string RewardText;
    public string InteractionText;

    void Start()
    {
        StoveIsLit = false;
        spawned = false;
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
        if(itemsLeft == 3)
        {
            BubbleText.text = InteractionText;
        }
        else if(itemsLeft < 3)
        {
            BubbleText.text = "Only " + itemsLeft + " ingredients left.";
        }

        // Check if this object has spawned its item yet
        if (!spawned)
        {
            if (HeldItem == (Pomegranate || LivingMossFlower || PaleTonic))
            {
                itemsLeft--;
                BubbleText.text = "Good Job! We only need " + itemsLeft + " more items!";
                Destroy(HeldItem);
                grabbed = false;
            }
        }

        if(itemsLeft == 0)
        {
            HeldItem = CultistWine;
            grabbed = true;
            spawned = true;
            BubbleText.text = RewardText;
        }
    }
}
