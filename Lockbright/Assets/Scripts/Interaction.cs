using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject requiremnt;
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

                // Is the player holding the right item?
                if (player.HeldItem == requiremnt)
                {
                    Destroy(player.HeldItem);
                    Instantiate(reward, spawnPoint, new Quaternion(0, 0, 0, 0));
                    spawned = true;
                }
            }
        }
    }
}
