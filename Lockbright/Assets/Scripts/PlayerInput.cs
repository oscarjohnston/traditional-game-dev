using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    //Movement
    private float xInput, yInput;
    public float SPEED = 1.0f;

    //Distinguish player's input from each other
    public int PlayerNumber;

    //Speech Bubble holders
    public GameObject SpeechBubble;
    public Text BubbleText;

    //Body holder
    private Rigidbody2D Body;

    //Player Switching mechanic
    private float INTERACTABLE_TIME = 1f;

    //Grabber Code
    public bool grabbed;
    public float distance = 5f;
    public Transform holdpoint;
    private bool PromptOn = false;
    private GameObject PromptObject = null;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Don't progress further with input if the prompt is on
        if (PromptOn)
        {
            // Confirm pickup
            if (Input.GetButtonDown("A_Button_" + PlayerNumber))
            {
                grabbed = true;
                HeldItem = PromptObject;
                SpeechBubble.SetActive(false);
                PromptOn = false;
            }
            // Deny pickup
            else if(Input.GetButtonDown("B_Button_" + PlayerNumber))
            {
                SpeechBubble.SetActive(false);
                PromptOn = false;
            }
            return;
        }

        // Change character input
        if (Input.GetButtonDown("LB_Button_" + PlayerNumber))
        {
            Invoke("ChangePlayerNumberDown", INTERACTABLE_TIME);
            return;
        }
        else if (Input.GetButtonDown("RB_Button_" + PlayerNumber))
        {
            Invoke("ChangePlayerNumberUp", INTERACTABLE_TIME);
            return;
        }

        // Input for movement
        xInput = Input.GetAxis("Horizontal" +  PlayerNumber);
        yInput = Input.GetAxis("Vertical" + PlayerNumber);
        var moveVector = new Vector3(xInput, yInput, 0) * SPEED * Time.deltaTime * 10;

        Body.MovePosition(new Vector2(transform.position.x + moveVector.x, transform.position.y + moveVector.y));


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

        // B Button Pressed
        if (Input.GetButtonDown("B_Button_" + PlayerNumber))
        {
            DropItem();
        }

        // A Button Pressed
        if (Input.GetButtonDown("A_Button_" + PlayerNumber) || Input.GetKeyDown(KeyCode.Space))
        {
            if (!grabbed)
            {
                //Raycast Zone of pickup after shifting the pickup orgin down
                Vector3 Adjustment = new Vector2(0, -0.5f);
                Collider2D[] collide = Physics2D.OverlapBoxAll(transform.position + Adjustment + dir, new Vector2(1, 1), 0);

                // Search each collision looking for an item to pickup
                foreach(Collider2D collision in collide)
                {
                    // Make sure you can actually pick item up before moving it
                    if (collision.tag == "Item")
                    {
                        // Prompt player to pick up item
                        SpeechBubble.SetActive(true);
                        BubbleText.text = "Do you want to pick this " + collision.name + " up?\n A : Pickup    B : Ignore";

                        PromptOn = true;
                        PromptObject = collision.gameObject;
                        return;
                    }
                }
            }
        }

        // X Button Pressed
        if (Input.GetButtonDown("X_Button_" + PlayerNumber))
        {
            print(this.name + " tried to inspect");
        }

        // Y Button Pressed
        if (Input.GetButtonDown("Y_Button_" + PlayerNumber))
        {
            print(this.name + " just used their ability");
        }

        // If an item is being held, then tell that game object to move to the player's hold point with the player's movement
        if (grabbed)
        {
            HeldItem.transform.position = holdpoint.position;
        }


        // Win the prototype
        if(HeldItem != null && HeldItem.name == "CharredKey")
        {
            WinGame.Invoke();
        }
    }

    /// <summary>
    /// This function will drop an item if there is one. This is done by dereferncing the item held
    /// </summary>
    void DropItem()
    {
        if (grabbed)
        {
            print("Dropped Item");
            HeldItem = null;
            grabbed = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print(this.name + " has collided with " + collision.collider.name);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // Interacte with what you're colliding with by pressing the A button
        if(Input.GetButtonDown("A_Button_" + PlayerNumber))
        {

            print(this.name + " has interacted with " + collision.collider.name);
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

    /// <summary>
    /// 1 -> 4
    /// 2 -> 1
    /// 3 -> 2
    /// 4 -> 3
    /// </summary>
    private int shiftRight(int PlayerNumber)
    {
        if(PlayerNumber == 1) { return 4; }
        else { return PlayerNumber - 1; }
    }

    /// <summary>
    /// 1 -> 2
    /// 2 -> 3
    /// 3 -> 4
    /// 4 -> 1
    /// </summary>
    private int shiftLeft(int PlayerNumber)
    {
        if (PlayerNumber == 4) { return 1; }
        else { return PlayerNumber + 1; }
    }

    private void ChangePlayerNumberUp()
    {
        GameObject.Find("Player 1").GetComponent<PlayerInput>().PlayerNumber = shiftRight(GameObject.Find("Player 1").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 2").GetComponent<PlayerInput>().PlayerNumber = shiftRight(GameObject.Find("Player 2").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 3").GetComponent<PlayerInput>().PlayerNumber = shiftRight(GameObject.Find("Player 3").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 4").GetComponent<PlayerInput>().PlayerNumber = shiftRight(GameObject.Find("Player 4").GetComponent<PlayerInput>().PlayerNumber);

        print("Players shift right");
    }

    private void ChangePlayerNumberDown()
    {
        GameObject.Find("Player 1").GetComponent<PlayerInput>().PlayerNumber = shiftLeft(GameObject.Find("Player 1").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 2").GetComponent<PlayerInput>().PlayerNumber = shiftLeft(GameObject.Find("Player 2").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 3").GetComponent<PlayerInput>().PlayerNumber = shiftLeft(GameObject.Find("Player 3").GetComponent<PlayerInput>().PlayerNumber);
        GameObject.Find("Player 4").GetComponent<PlayerInput>().PlayerNumber = shiftLeft(GameObject.Find("Player 4").GetComponent<PlayerInput>().PlayerNumber);

        print("Players shift left");
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
