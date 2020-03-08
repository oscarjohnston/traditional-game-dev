﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    //Movement
    private float xInput, yInput;
    public float SPEED = 1.0f;

    //Distinguish players from each other
    public int PlayerNumber = 1;

    //Speech Bubble holders
    public GameObject SpeechBubble;
    public Text BubbleText;
    private bool Interactable;

    //Body holder
    private Rigidbody2D Body;

    //Player Switching mechanic
    private bool AbleToChangePlayerNumber;
    private float INTERACTABLE_TIME = 1f;

    //Grabber Code
    public bool grabbed;
    public float distance = 5f;
    public Transform holdpoint;

    Vector3 previousGood = Vector3.zero;

    public GameObject HeldItem;

    //Unity Events to win prototype
    public UnityEvent InteractWithStove;
    public UnityEvent InteractWithFridge;
    public UnityEvent WinGame;

    // Start is called before the first frame update
    void Start()
    {
        //Sets up body
        Body = GetComponent<Rigidbody2D>();

        //Sets up ability to change player
        Interactable = true;
        AbleToChangePlayerNumber = true;
    }

    // Update is called once per frame
    void Update()
    {

        // Input for movement
        xInput = Input.GetAxis("Horizontal" +  PlayerNumber);
        yInput = Input.GetAxis("Vertical" + PlayerNumber);
        var moveVector = new Vector3(xInput, yInput, 0) * SPEED * Time.deltaTime * 10;

        Body.MovePosition(new Vector2(transform.position.x + moveVector.x, transform.position.y + moveVector.y));

        // Trying to change character input
        if(AbleToChangePlayerNumber && Input.GetButton("LB_Button_" + PlayerNumber))
        {
            //print("Player " + PlayerNumber + " has pressed the LB Button");
            ChangePlayerNumberDown();
        }
        else if(AbleToChangePlayerNumber && Input.GetButton("RB_Button_" + PlayerNumber))
        {
            //print("Player " + PlayerNumber + " has pressed the RB Button");
            ChangePlayerNumberUp();
        }

        //Grabber code
        Vector3 dir = new Vector2(xInput, yInput);
        if (dir == Vector3.zero)
        {
            dir = previousGood;
        }
        else
        {
            previousGood = dir;
        }

        if (Input.GetButton("A_Button_" + PlayerNumber) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!grabbed)
            {
                //Raycast Zone of pickup after shifting the pickup orgin down
                Vector3 Adjustment = new Vector2(0, -0.5f);
                Collider2D[] collide = Physics2D.OverlapBoxAll(transform.position + Adjustment + dir, new Vector2(1, 1), 0);

                // Search each collision looking for an item to pickup
                foreach(Collider2D collision in collide)
                {
                    //Makes sure you can actually pick item up before moving it
                    if (collision.tag == "Item")
                    {
                        grabbed = true;
                        HeldItem = collision.gameObject;
                    }
                }
                               

                
            }
            //else if(!Physics2D.OverlapPoint(holdpoint.position))
            else
            {
                print("Dropped Item");
                grabbed = false;
                HeldItem = null;
            }

        }

        //Moves item to holdpoint
        if (grabbed)
        {
            HeldItem.transform.position = holdpoint.position;
        }


        // Win the protoype
        if(HeldItem != null && HeldItem.name == "CharredKey")
        {
            WinGame.Invoke();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("Player " + PlayerNumber + " has collided with " + collision.collider.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //ToggleSpeechBubble(false);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Interacte with what you're colliding with by pressing the A button
        if(Interactable && Input.GetButton("A_Button_" + PlayerNumber))
        {
            Interactable = false;

            // Set Interactable to true after wait
            Invoke("SetInteractable", INTERACTABLE_TIME);

            print("Player " + PlayerNumber + " has interacted with " + collision.collider.name);
            ActivateSpeechBubble();
            BubbleText.text = "It's a " + collision.collider.name;

            if(collision.gameObject.name == "Stove" && HeldItem.name == "Books")
            {
                print("Interacted with Stove, spawning key");
                BubbleText.text = "Yay! A Key!";
                InteractWithStove.Invoke();

                HeldItem = null;
                grabbed = false;
            }
            else if(collision.gameObject.name == "Fridge")
            {
                print("Interacted with Fridge, spawning books on floor");
                BubbleText.text = "Books for the stove fell";
                InteractWithFridge.Invoke();
            }
        }
    }

    private void SetInteractable()
    {
        Interactable = true;
    }

    private void ChangePlayerNumberUp()
    {
        if(AbleToChangePlayerNumber && PlayerNumber != 4)
        {
            AbleToChangePlayerNumber = false;
            PlayerNumber++;

            // Set Interactable to true after 0.2 seconds
            Invoke("ActivatePlayerNumberChange", 1.0f);

            print("Increased Player Number");
        }
        else
        {
            AbleToChangePlayerNumber = false;
            PlayerNumber = 1;

            // Set Interactable to true after 0.2 seconds
            Invoke("ActivatePlayerNumberChange", 1.0f);

            print("Increased Player Number");
        }
    }

    private void ChangePlayerNumberDown()
    {
        if (AbleToChangePlayerNumber && PlayerNumber != 1)
        {
            AbleToChangePlayerNumber = false;
            PlayerNumber--;

            // Set Interactable to true after 0.2 seconds
            Invoke("ActivatePlayerNumberChange", 1.0f);

            print("Decreased Player Number");
        }
        else
        {
            AbleToChangePlayerNumber = false;
            PlayerNumber = 4;

            // Set Interactable to true after 0.2 seconds
            Invoke("ActivatePlayerNumberChange", 1.0f);

            print("Increased Player Number");
        }
    }

    private void ActivatePlayerNumberChange()
    {
        AbleToChangePlayerNumber = true;
    }

    private void ActivateSpeechBubble()
    {
        SpeechBubble.SetActive(true);
        Invoke("DeactivateSpeechBubble", INTERACTABLE_TIME);
    }

    private void DeactivateSpeechBubble()
    {
        SpeechBubble.SetActive(false);
    }

}
