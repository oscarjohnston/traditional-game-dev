using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject requirement;
    public string PlayerRequirement;

    public GameObject reward;
    public Vector3 spawnPoint;
    public bool spawned;
    public string bubbleText;

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
        if (!spawned && collision.gameObject.tag == "Player")
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            if(player != null && Input.GetButtonDown("A_Button_" + player.PlayerNumber))
            {
                player.BubbleText.text = bubbleText;

                // Is the player holding the right item? (and optionally the correct Player?)
                if (player.HeldItem == requirement && (PlayerRequirement != null && player.name.Equals(PlayerRequirement)))
                {
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
    }
}
