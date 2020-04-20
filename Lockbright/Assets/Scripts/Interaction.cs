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

    // Start is called before the first frame update
    void Start()
    {
        spawned = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        BubbleText.text = InteractionText;

        // turn sink "Hot" in about ten seconds
        if (Sink && !(working))
        {
            Invoke("SinkIsHot", 10f);
        }

        // move parkourist from ladder to ladder
        if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && loungeLadder)
        {
            player.transform.position = studyLocation;
        }
        if (PlayerRequirement != null && Class.Equals(PlayerRequirement) && studyLadder)
        {
            player.transform.position = loungeLocation;
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
                                //play music
                                song.Play();

                                Invoke("bingbong", 16f);

                                HeldItem = reward;
                                HeldItem.GetComponent<HeldItems>().CanPickThisUp = true;
                                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
                                grabbed = true;
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

    void bingbong()
    {

    }
}

