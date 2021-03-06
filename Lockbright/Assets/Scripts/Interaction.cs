﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public GameController game_controller;
    public FridgeMonster fridgeMonster;
    public GameObject[] requirement;
    public string PlayerRequirement;

    public GameObject reward;
    public Vector3 spawnPoint;
    public bool spawned;
    public string RewardText;
    public string InteractionText;
    public bool SpawnsAMonster = false;

    // Added for bookshelfs
    public GameObject[] returns;
    public bool IsBookshelf;

    // Added for Ladders
    public bool loungeLadder;
    public bool studyLadder;
    public GameObject player;
    public GameObject studyLocation;
    public GameObject loungeLocation;

    // Record player
    public bool musicPlayer;
    public AudioSource song;

    // Study Door stuff
    public bool StudyDoor;

    // Trunk Puzzle
    private int TrunkItemsRemaining = 2;

    // Front Door Puzzle
    private int FrontDoorItemsRemaining = 4;

    // Generic Requirement Puzzle Counter
    private int GenericItemsRemaining;

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
        GenericItemsRemaining = requirement.Length;

        // Setup the gmae controller at the start for all interaction objects
        game_controller = GameObject.Find("Game Controller").GetComponent<GameController>();
    }

    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        BubbleText.text = InteractionText;

        // Handle the closed trunk to turn into the open trunk
        if (this.gameObject.name == "Trunk")
        {
            BubbleText.text = RewardText;

            // Place the open trunk where this trunk was, shift it down 5.2 exactly
            returns[0].transform.position = transform.position + new Vector3(0, 5.2f, 0);

            // Place reward off to the left
            reward.transform.position = this.gameObject.transform.position + new Vector3(-20, 0, 0);

            // Make sure the reward has been set for pickup and now exists on the Player sorting layer
            reward.GetComponent<HeldItems>().CanPickThisUp = true;
            reward.GetComponent<SpriteRenderer>().sortingLayerName = "Player";

            // Destroy the closed trunk
            Destroy(this.gameObject);

            return;
        }
        else if(this.gameObject.name == "Sink")
        {
            if(!spawned && HeldItem != null && (HeldItem == requirement[0] || HeldItem == requirement[1]))
            {
                // Take the item
                Destroy(HeldItem.gameObject);
                grabbed = false;

                // Give Reward Item
                spawned = true;
                HeldItem = reward;
                grabbed = true;
                HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                BubbleText.text = RewardText;

                // Spawn the monster at the sink
                fridgeMonster.SpawnMonster();
            }
        }

        else if (this.gameObject.name == "Opened Trunk and Sun Puzzle")
        {
            print("Trying to do the sun puzzle");

            // If the player isn't holding an item then just return
            if (HeldItem == null)
            {
                return;
            }

            // If not spawned and held item matches one of the requirements
            if (!spawned && (HeldItem == requirement[0] || HeldItem == requirement[1]))
            {
                // Activate the visuals
                if(HeldItem.name == "West Sun Charm")
                {
                    game_controller.ActivateWestCharm();
                }
                else if(HeldItem.name == "East Sun Charm")
                {
                    game_controller.ActivateEastCharm();
                }

                // Delete the held item and decrement the required items
                TrunkItemsRemaining--;
                Destroy(HeldItem.gameObject);
                grabbed = false;

                BubbleText.text = "I was able to place one of the charms";

                // Handle if the items required is now zero to give the sun key
                if (TrunkItemsRemaining == 0)
                {
                    print("Trunk Puzzle Solved");

                    spawned = true;
                    HeldItem = reward;
                    grabbed = true;
                    HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                    HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                    BubbleText.text = RewardText;

                }
            }

            return;
        }
        else if (this.gameObject.name == "Front Door")
        {
            print("Trying to leave at front door");

            // If the player isn't holding an item then just return
            if (HeldItem == null)
            {
                return;
            }

            // If not spawned and held item matches one of the requirements
            if (!spawned && (HeldItem == requirement[0] || HeldItem == requirement[1] || HeldItem == requirement[2] || HeldItem == requirement[3]))
            {
                // Delete the held item and decrement the required items
                FrontDoorItemsRemaining--;
                Destroy(HeldItem.gameObject);
                grabbed = false;

                BubbleText.text = "I was able to get one of the locks! Just " + FrontDoorItemsRemaining + " keys to go!";

                // Handle if the items required is now zero to win the game
                if (FrontDoorItemsRemaining == 0)
                {
                    BubbleText.text = "We Escaped!";
                    game_controller.InvokeWinGameEvent();
                }
            }

            return;
        }

        else if (this.gameObject.name == "Sink (Bathroom)")
        {
            if(game_controller.IsBoilerOn() && HeldItem == requirement[0])
            {
                // Remove the fireball
                Destroy(HeldItem.gameObject);

                // Give the player the reward item
                HeldItem = reward;
                HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                grabbed = true;
                spawned = true;

                BubbleText.text = RewardText;
            }
            // Activate sink in 20 seconds
            else if(!game_controller.IsBoilerOn())
            {
                Invoke("SinkIsHot", 20f);
                BubbleText.text = "Looks like the water will take a while to heat up";
                return;
            }
        }

        // move parkourist from ladder to ladder
        else if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && loungeLadder)
        {
            player.transform.position = studyLocation.transform.position;
            BubbleText.text = RewardText;
            return;
        }
        else if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && studyLadder)
        {
            player.transform.position = loungeLocation.transform.position;
            BubbleText.text = RewardText;
            return;
        }

        // Literally just remove the door lol
        else if (StudyDoor && HeldItem == requirement[0])
        {
            BubbleText.text = RewardText;
            Destroy(this.gameObject);
            Destroy(HeldItem);
            grabbed = false;

            return;
        }

        // Check if this object has spawned its item yet
        if (!spawned)
        {
            // Is the player holding the right item? (and optionally the correct Player?)
            if (requirement.Length == 0)
            {
                // If it's the wrong player requirement, do nothing
                if (PlayerRequirement != "" && !Class.Equals(PlayerRequirement))
                {
                    return;
                }
                else
                {
                    BubbleText.text = RewardText;
                    spawned = true;

                    // If the player is already holding an item, spawn the item at spawnpoint 0
                    if (HeldItem != null && reward != null)
                    {
                        reward.GetComponent<HeldItems>().CanPickThisUp = true;
                        reward.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                        reward.transform.position = spawnPoint;
                    }
                    // Otherwise if there's a reward, give it to the player
                    else if (reward != null)
                    {
                        HeldItem = reward;
                        HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                        HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                        grabbed = true;
                    }
                }

                // Specific case to activate the ladder system
                if (this.gameObject.name == "Right Lounge Bookcase")
                {
                    game_controller.ActivateLadderSystem();
                }
            }
            else
            {
                foreach (GameObject required in requirement)
                {
                    // Handle when a required item has been fulfilled
                    if(required == null) { continue; }

                    if (HeldItem == required || (PlayerRequirement != null && Class.Equals(PlayerRequirement)))
                    {
                        // Double check the held item before it's destroyed for the moon puzzle visuals
                        if(HeldItem.name == "Crescent Moon Brick")
                        {
                            game_controller.ActivateCrescentMoonBrick();
                        }
                        else if (HeldItem.name == "Half Moon Brick")
                        {
                            game_controller.ActivateHalfMoonBrick();
                        }

                        // This held item is one of the required items, so decrement the counter and figure out if the requirement is met
                        GenericItemsRemaining--;

                        // Take the player's item and clear their held item status
                        Destroy(HeldItem);
                        grabbed = false;

                        // If it's a bookshelf, need to tell the game controller to decrement the count for dusty record
                        if (IsBookshelf)
                        {
                            print("Its a bookshelf, telling game controller to decrement counter");
                            game_controller.DecrementDustyRecordCounter();
                            
                            switch (HeldItem.name)
                            {
                                case "Book Of Seeds":
                                    game_controller.ActivateSeeds();
                                    return;
                                case "Book Of Sprouts":
                                    game_controller.ActivateSprouts();
                                    return;
                                case "Book Of Flowers":
                                    game_controller.ActivateFlowers();
                                    return;
                                case "Book Of Fruit":
                                    game_controller.ActivateFruit();
                                    return;
                            }
                        }

                        // If this item is not yet ready to be awarded, put in some text and return
                        if (GenericItemsRemaining > 1)
                        {
                            BubbleText.text = "Nice! This only requires " + GenericItemsRemaining + " more items.";
                            return;
                        }
                        else if (GenericItemsRemaining == 1)
                        {
                            BubbleText.text = "Nice! This requires 1 more item.";
                            return;
                        }

                        // Otherwise, the requirement is at 0 and ready for reward
                        BubbleText.text = RewardText;

                        // Handle Specific object's activation
                        if (musicPlayer)
                        {
                            print("Activating Music Player");

                            spawned = true;

                            //play music
                            song.Play();

                            Invoke("DropStudyKey", 16f);

                            // Move monster
                            fridgeMonster.SpawnMonster();

                            return;
                        }

                        if (reward != null)
                        {
                            HeldItem = reward;
                            HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                            HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                            grabbed = true;
                            spawned = true;
                        }

                        else if (this.gameObject.name == "Broken Boiler")
                        {
                            game_controller.CanTurnTheBoilerOn();
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Calls the game controller to turn on the boiler because the sink has heated up
    /// </summary>
    void SinkIsHot()
    {
        game_controller.SinkTurnOnBoilerOverride();
    }

    void DropStudyKey()
    {
        // Move the key over to the music box's location instead of the player's inventory, adjusted down a bit
        reward.transform.position = this.gameObject.transform.position + new Vector3(-15, 0, 0);

        // Make sure the reward has been set for pickup and now exists on the Player sorting layer
        reward.GetComponent<HeldItems>().CanPickThisUp = true;
        reward.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }
}

