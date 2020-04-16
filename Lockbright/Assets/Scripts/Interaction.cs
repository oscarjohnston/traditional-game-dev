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

    public bool Boiler;
    public bool Sink;

    public bool working;

    public Interaction preReq;

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

        if (Sink && !(working))
        {
            Invoke("SinkIsHot", 10f);
        }

        // Check if this object has spawned its item yet
        if (!spawned)
        {
            // Is the player holding the right item? (and optionally the correct Player?)
            if(requirement.Length == 0)
            {
                if (PlayerRequirement != null && Class.Equals(PlayerRequirement))
                {
                    BubbleText.text = RewardText;

                    Destroy(HeldItem);
                    if (reward != null && working)
                    {
                        HeldItem = reward;
                        grabbed = true;
                    }
                    else
                    {
                        grabbed = false;
                    }
                    //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                    spawned = true;
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
                            grabbed = true;
                        }
                        else
                        {
                            grabbed = false;
                        }
                        //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                        spawned = true;

                        if (Boiler)
                        {
                            working = true;
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

    public bool IsWorking()
    {
        return working;
    }
}

