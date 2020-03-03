using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject requiremnt;
    public GameObject reward;
    public Transform spawnPoint;
    public bool spawned;
    public string bubbleText = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
       PlayerInput player = collision.collider.GetComponent<PlayerInput>();

        player.BubbleText.text = bubbleText;

        if (player != null)
        {
            // Is the player holding the right item?
            if (player.HeldItem == requiremnt)
            {
                Destroy(player.HeldItem);
                GameObject newItem = Instantiate(reward);
                player.HeldItem = newItem;
            }
        }

        /*
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !spawned)
        {
            print("Interacted with fridge, spawning book");
            Instantiate(reward, spawnPoint, true);
            spawned = true;
        }
        */
    }
}
