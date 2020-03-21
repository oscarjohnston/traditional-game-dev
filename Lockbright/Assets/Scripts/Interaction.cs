using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public GameObject requirement;
    public string PlayerRequirement;

    public GameObject reward;
    public Vector3 spawnPoint;
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

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*

        // Check if this object has spawned its item yet, then make sure it's colliding with a player
        if (!spawned && collision.gameObject.tag == "Player")
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            // If the colliding player exists...
            // And that player has pressed the A button...
            if(player != null && Input.GetButtonDown("A_Button_" + player.PlayerNumber))
            {
                player.BubbleText.text = InteractionText;

                // Is the player holding the right item? (and optionally the correct Player?)
                if (player.HeldItem == requirement && (PlayerRequirement != null && player.name.Equals(PlayerRequirement)))
                {
                    player.BubbleText.text = RewardText;

                    Destroy(player.HeldItem);
                    if(reward != null) 
                    { 
                        player.HeldItem = reward;
                    }
                    else
                    {
                        player.grabbed = false;
                    }
                    //Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                    spawned = true;
                }
            }
        }

        */
    }

    public void TryToInteractWithThisObject(string Class, ref GameObject HeldItem, ref Text BubbleText, ref bool grabbed)
    {
        BubbleText.text = InteractionText;

        // Check if this object has spawned its item yet
        if (!spawned)
        {
                // Is the player holding the right item? (and optionally the correct Player?)
                if (HeldItem == requirement && (PlayerRequirement != null && Class.Equals(PlayerRequirement)))
                {
                    BubbleText.text = RewardText;

                    Destroy(HeldItem);
                    if (reward != null)
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
    }
}
