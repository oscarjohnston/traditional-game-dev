using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveScript : MonoBehaviour
{
    // Controller for particle
    public UIController game;

    // Boolean for identifying if the stove is on
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
    
    public void LightOrUnlightTheStove()
    {
        // Try to turn the stove on if it's not lit yet
        if (!StoveIsLit)
        {
            StoveIsLit = true;
            game.FireTurnOnStove();

            print("Turning on stove");
        }

        // Otherwise turn it off
        else
        {
            StoveIsLit = false;
            game.FireTurnOffStove();

            print("Turning off stove");
        }
    }

    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        if(itemsLeft == 3)
        {
            BubbleText.text = InteractionText;
        }
        else if (itemsLeft == 0)
        {
            BubbleText.text = "Now we just need to heat it up!";
        }
        else if(itemsLeft < 3)
        {
            BubbleText.text = "Only " + itemsLeft + " ingredients left.";
        }

        // Check if this object has spawned its item yet
        if (!spawned && itemsLeft != 0)
        {
            if (HeldItem == (Pomegranate || LivingMossFlower || PaleTonic))
            {
                itemsLeft--;
                Destroy(HeldItem);
                grabbed = false;

                if (itemsLeft > 0) { BubbleText.text = "Good Job! We only need " + itemsLeft + " more items!"; }
                else { BubbleText.text = "Now we just need to heat it up!"; }
            }
        }

        // Only give the cultist wine if the stove is lit, and all the items have been aquired
        if(itemsLeft == 0 && StoveIsLit)
        {
            HeldItem = CultistWine;
            grabbed = true;
            spawned = true;
            BubbleText.text = RewardText;
        }
    }
}
