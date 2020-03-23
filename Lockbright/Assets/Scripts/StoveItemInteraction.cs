using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoveItemInteraction : MonoBehaviour
{
    public GameObject PaleTonic;
    public GameObject LivingMossFlower;
    public GameObject Pomegranate;

    public StoveScript stove;

    public int itemsleft = 3;

    public GameObject reward;
    public bool spawned;
    public string RewardText;
    public string InteractionText;

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
        // No items added gives first message
        if(itemsleft == 3)
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


            if (reward != null && itemsleft == 0)
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
