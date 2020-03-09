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

    // Speech Bubble holders
    public GameObject SpeechBubble;
    public Text BubbleText;

    // Body holder
    private Rigidbody2D Body;

    // Player Switching mechanic
    private float INTERACTABLE_TIME = 1f;

    // Scholar's Speechbubble reading time
    private float SCHOLAR_BUBBLE_TIME = 4f;

    //Grabber Code
    public bool grabbed;
    public float distance = 5f;
    public Transform holdpoint;
    private bool PickupPromptOn = false;
    private GameObject PickupPromptObject = null;
    private bool SwapPromptOn = false;
    private GameObject SwapPromptObject = null;

    Vector3 previousGood = Vector3.zero;

    public GameObject HeldItem;

    // Unity Events to win prototype
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
        // First look for any prompts
        // Pickup Prompt
        if (PickupPromptOn)
        {
            // Confirm pickup
            if (Input.GetButtonDown("A_Button_" + PlayerNumber))
            {
                grabbed = true;
                HeldItem = PickupPromptObject;
                SpeechBubble.SetActive(false);
                PickupPromptOn = false;

                // TODO: Put into UI Inventory
            }
            // Deny pickup
            else if(Input.GetButtonDown("B_Button_" + PlayerNumber))
            {
                SpeechBubble.SetActive(false);
                PickupPromptOn = false;
            }
            return;
        }

        // Swap Prompt
        else if (SwapPromptOn)
        {
            // Confirm pickup
            if (Input.GetButtonDown("A_Button_" + PlayerNumber))
            {
                // Swap the location of the held item with the swap item
                HeldItem.transform.position = SwapPromptObject.transform.position;

                // Pickup the new item
                HeldItem = SwapPromptObject;
                SpeechBubble.SetActive(false);
                SwapPromptOn = false;

                // TODO: Put into UI Inventory
            }
            // Deny pickup
            else if (Input.GetButtonDown("B_Button_" + PlayerNumber))
            {
                SpeechBubble.SetActive(false);
                SwapPromptOn = false;
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
            // TODO: Take out of UI Inventory
        }

        // X Button Pressed
        if (Input.GetButtonDown("X_Button_" + PlayerNumber))
        {
            if(grabbed)
            {
                SpeechBubble.SetActive(true);
                BubbleText.text = "Looks like I'm holding " + HeldItem.name;
                Invoke("DeactivateSpeechBubble", 4f);
            }
        }

        // A Button Pressed
        if (Input.GetButtonDown("A_Button_" + PlayerNumber) || Input.GetKeyDown(KeyCode.Space))
        {
            //Raycast Zone of pickup after shifting the pickup orgin down
            Vector3 Adjustment = new Vector2(0, -0.5f);
            Collider2D[] collide = Physics2D.OverlapBoxAll(transform.position + Adjustment + dir, new Vector2(1, 1), 0);

            // Search each collision looking for an item to pickup
            foreach (Collider2D collision in collide)
            {
                // Make sure you can actually pick item up before moving it
                if (collision.tag == "Item")
                {
                    if (!grabbed)
                    {
                        // Prompt player to pick up item
                        SpeechBubble.SetActive(true);
                        BubbleText.text = "Do you want to pick this " + collision.name + " up?\n A : Pickup    B : Ignore";

                        PickupPromptOn = true;
                        PickupPromptObject = collision.gameObject;
                        return;
                    }
                    else
                    {
                        // Prompt player to swap item
                        SpeechBubble.SetActive(true);
                        BubbleText.text = "Do you want to swap for the " + collision.name + "?\n A : Swap    B : Ignore";

                        SwapPromptOn = true;
                        SwapPromptObject = collision.gameObject;
                        return;
                    }
                }
            }
        }

        // Y Button Pressed
        if (Input.GetButtonDown("Y_Button_" + PlayerNumber))
        {
            print(this.name + " just used their ability");

            // TODO: Activate particle effects for pressing Y

            // Call the correct ability based on object name
            switch (this.gameObject.name)
            {
                case "Player 1":
                    DoScholarAbility();
                    return;
                case "Player 2":
                    DoBurnerAbility();
                    return;
                case "Player 3":
                    DoParkouristAbility();
                    return;
                case "Player 4":
                    DoIllusionistAbility();
                    return;
            }
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
    /// The burner will activate a glow. It will be the whitebox's scripts responsibility to fulfill burner requirements for events
    /// </summary>
    void DoBurnerAbility()
    {
        // TODO: Activate glow effect
    }

    /// <summary>
    /// The Parkourist will get a burst of speed for 1 second
    /// </summary>
    void DoParkouristAbility()
    {
        this.SPEED *= 2;
        Invoke("TurnOffSpeedBoost", 1f);
    }

    void TurnOffSpeedBoost()
    {
        this.SPEED /= 2;
    }

    /// <summary>
    /// The scholar's ability depends on the item held. If the item is one of the correct items, then a dialog will popup.
    /// If there is no item held, then no dialog
    /// </summary>
    void DoScholarAbility()
    {
        if (grabbed)
        {
            SpeechBubble.SetActive(true);
            Invoke("DeactivateSpeechBubble", SCHOLAR_BUBBLE_TIME);

            switch (HeldItem.name)
            {
                case "Books":
                    BubbleText.text = "Whatever the book says";
                    return;
                case "Cookbook":
                    BubbleText.text = "Cultist Wine: Tastes much like blood and is attractive to water-dwellers. Combine Pale Tonic, Fresh Mossflower, and Pomegranate together; then warm to body temperature.";
                    return;
                default:
                    BubbleText.text = "This item can't be read";
                    return;
            }
        }
        
    }

    /// <summary>
    /// The Illisionist will create a healing radius that heals 1 health point
    /// </summary>
    void DoIllusionistAbility()
    {
        // TODO: Figure out healing radius
        // TODO: Fire off healing glow effect
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
