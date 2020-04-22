using System.Collections;
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
    public bool CanWinTheGame = false;
    public bool SpawnsAMonster = false;

    // Added for Sink and Boiler
    public bool Boiler;
    public bool Sink;

    public bool working;

    // Added for bookshelfs
    public bool finalShelf;
    public Interaction preReq;
    public GameObject[] returns;
    public Vector3[] spawnpoints;

    // Added for Ladders
    public bool loungeLadder;
    public bool studyLadder;
    public GameObject player;
    public Vector3 studyLocation;
    public Vector3 loungeLocation;

    // Record player
    public bool musicPlayer;
    public AudioSource song;

    // Study Door stuff
    public bool StudyDoor;

    // Stairs
    public bool landing;
    public bool mainHall;
    public Vector3 landingLocation;
    public Vector3 mainHallLocation;

    // Trunk Puzzle
    private int TrunkItemsRemaining = 2;

    // Front Door Puzzle
    private int FrontDoorItemsRemaining = 4;

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
    }

    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        BubbleText.text = InteractionText;

        // Handle the closed trunk to turn into the open trunk
        if(this.gameObject.name == "Trunk")
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
        else if(this.gameObject.name == "Opened Trunk and Sun Puzzle")
        {
            print("Trying to do the sun puzzle");

            // If not spawned and held item matches one of the requirements
            if (!spawned && (HeldItem == requirement[0] || HeldItem == requirement[1]))
            {
                // Delete the held item and decrement the required items
                TrunkItemsRemaining--;
                Destroy(HeldItem.gameObject);
                grabbed = false;

                BubbleText.text = "I was able to place one of the charms";

                // Handle if the items required is now zero to give the sun key
                if(TrunkItemsRemaining == 0)
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
        else if(this.gameObject.name == "Front Door")
        {
            print("Trying to leave at front door");

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
        // TODO: Change all the objects to Bookcase so it can all go through this one thing
        else if(this.gameObject.name == "BookCase")
        {
            game_controller.DecrementBookshelfCounter();

            //TODO: Implement rest of book requirements and rewards, bubble text etc.
        }

        // turn sink "Hot" in about ten seconds
        if (Sink && !(working))
        {
            Invoke("SinkIsHot", 10f);
            return;
        }

        // move parkourist from ladder to ladder
        if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && loungeLadder)
        {
            player.transform.position = studyLocation;
            return;
        }
        if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && studyLadder)
        {
            player.transform.position = loungeLocation;
            return;
        }

        // Move Player from stair to stair
        if (landing)
        {
            //move to mainhall

            return;
        }
        if (mainHall)
        {
            //move to landing

            return;
        }

        // Literally just remove the door lol
        if(StudyDoor && HeldItem == requirement[0])
        {
            BubbleText.text = RewardText;
            Destroy(this.gameObject);
            Destroy(HeldItem);
            grabbed = false;

            return;
        }

        // preReq for bookshelfs to work off of each other
        if (preReq != null && preReq.spawned)
        {
            // Check if this object has spawned its item yet
            if (!spawned)
            {
                // Is the player holding the right item? (and optionally the correct Player?)
                if (requirement.Length == 0)
                {
                    if (PlayerRequirement != null && Class.Equals(PlayerRequirement))
                    {
                        BubbleText.text = RewardText;

                        Destroy(HeldItem);
                        if (reward != null && working)
                        {
                            HeldItem = reward;
                            HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                            HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                            grabbed = true;
                            spawned = true;
                        }
                        else if (working)
                        {
                            grabbed = false;
                            spawned = true;
                        }
                        //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                    }
                }
                else
                {
                    foreach (GameObject required in requirement)
                    {
                        if (HeldItem == required || (PlayerRequirement != null && Class.Equals(PlayerRequirement)))
                        {
                            // This will fire the game winning event
                            if (CanWinTheGame)
                            {
                                game_controller.InvokeWinGameEvent();
                            }
                            else if (SpawnsAMonster)
                            {
                                fridgeMonster.SpawnMonster();
                            }

                            BubbleText.text = RewardText;

                            Destroy(HeldItem);
                            if (reward != null && working)
                            {
                                HeldItem = reward;
                                HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                grabbed = true;
                                spawned = true;
                            }
                            else if (working)
                            {
                                grabbed = false;
                                spawned = true;
                            }
                            //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));

                            if (Boiler)
                            {
                                working = true;
                            }

                            if (finalShelf)
                            {
                                for (int i = 0; i < returns.Length; i++)
                                {
                                    Instantiate(returns[i], spawnpoints[i], new Quaternion(0, 0, 0, 0));
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            // Check if this object has spawned its item yet
            if (!spawned)
            {
                // Is the player holding the right item? (and optionally the correct Player?)
                if (requirement.Length == 0)
                {
                    if (PlayerRequirement != null && Class.Equals(PlayerRequirement))
                    {
                        BubbleText.text = RewardText;

                        Destroy(HeldItem);
                        if (reward != null && working)
                        {
                            HeldItem = reward;
                            HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                            HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                            grabbed = true;
                            spawned = true;
                        }
                        else if (working)
                        {
                            grabbed = false;
                            spawned = true;
                        }
                        //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                    }
                }
                else
                {
                    foreach (GameObject required in requirement)
                    {
                        if (HeldItem == required || (PlayerRequirement != null && Class.Equals(PlayerRequirement)))
                        {
                            // This will fire the game winning event
                            if (CanWinTheGame)
                            {
                                game_controller.InvokeWinGameEvent();
                            }
                            else if (SpawnsAMonster)
                            {
                                fridgeMonster.SpawnMonster();
                            }

                            BubbleText.text = RewardText;

                            Destroy(HeldItem);
                            if (reward != null && working)
                            {
                                HeldItem = reward;
                                HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                grabbed = true;
                                spawned = true;
                            }
                            else if (working)
                            {
                                grabbed = false;
                                spawned = true;
                            }
                            //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));

                            if (Boiler)
                            {
                                working = true;
                                preReq.working = true;
                            }

                            
                            if (musicPlayer)
                            {
                                print("Activating Music Player");

                                spawned = true;
                                grabbed = false;

                                //play music
                                song.Play();

                                Invoke("DropStudyKey", 16f);
                            }
                        }
                    }
                }
            }
        }

    }

    void SinkIsHot()
    {
        working = true;
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

