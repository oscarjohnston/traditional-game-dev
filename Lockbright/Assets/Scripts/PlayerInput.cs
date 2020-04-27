using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    // Controllers
    public UIController ui_Controller;
    public GameController game_controller;

    // Movement
    private float xInput, yInput;
    public float SPEED = 1.0f;

    // Distinguish player's input from each other
    public int PlayerNumber;

    // Describe this player's class for interaction requirements
    public string Class;

    // This variable is to keep the Parkourist from doubling her speed over and over
    private bool ParkourCanDouble;

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
    public float distance = 50f;
    public Transform holdpoint;
    private bool PickupPromptOn = false;
    private GameObject PickupPromptObject = null;
    private bool SwapPromptOn = false;
    private GameObject SwapPromptObject = null;

    Vector3 previousGood = Vector3.zero;

    public GameObject HeldItem;

    // Sprite UI
    public Sprite defaultItem;
    public Image HeldItemImage;

    // Interactable variables
    private GameObject InteractingObject;
    private Interaction InteractingWith;
    public bool Interacting;
    // Optionally Stove
    private StoveScript StoveInteractingWith;
    public bool StoveInteracting;

    // Unity Events to win prototype
    public UnityEvent InteractWithStove;
    public UnityEvent InteractWithFridge;

    // Burner Glow Light
    public Light BurnerGlowLight;

    // Health
    public int health;
    public Slider HealthBar;
    private Vector3 StartingPosition;
    private bool Dead;

    // Animators
    public Animator front;
    public Animator side;
    public Animator back;
    private bool IsWalking;

    public UnityEvent FireParticle;

    // Fireball Boolean
    private bool CanBeBurned;

    // Start is called before the first frame update
    void Start()
    {
        //Sets up body
        Body = GetComponent<Rigidbody2D>();

        // Allow Parkourist to double from the start
        ParkourCanDouble = true;

        // Zero out the glow instensity
        BurnerGlowLight.intensity = 0;

        // Set Up Health
        health = 5;

        // Set the image to front
        front.gameObject.SetActive(true);
        back.gameObject.SetActive(false);
        side.gameObject.SetActive(false);

        IsWalking = false;

        StartingPosition = this.transform.position;
        Dead = false;

        CanBeBurned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead)
        {
            return;
        }

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

                // Put into UI Inventory
                HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;

                // Turn off the item
                HeldItem.SetActive(false);

                // Whatever sort group the item was before, just make it be the player level now
                HeldItem.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
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
                HeldItem.SetActive(true);

                // Pickup the new item
                HeldItem = SwapPromptObject;
                SpeechBubble.SetActive(false);
                SwapPromptOn = false;

                // Put into UI Inventory
                HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;

                // Turn off the item
                HeldItem.SetActive(false);
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
            Invoke("ChangePlayerNumberDown", 0.2f);
            return;
        }
        else if (Input.GetButtonDown("RB_Button_" + PlayerNumber))
        {
            Invoke("ChangePlayerNumberUp", 0.2f);
            return;
        }

        

        // Input for movement
        xInput = Input.GetAxis("Horizontal" +  PlayerNumber);
        yInput = Input.GetAxis("Vertical" + PlayerNumber);

        // Handle the player moving horizontally
        if (xInput >= 0.5f)
        {
            // Moving Right
            IsWalking = true;

            // Toggle animation
            front.SetBool("Walk", IsWalking);
            side.SetBool("Walk", IsWalking);
            back.SetBool("Walk", IsWalking);

            if (Class == "Illusionist")
            {
                side.transform.localScale = new Vector3(Math.Abs(side.transform.localScale.x), side.transform.localScale.y, side.transform.localScale.z);
            }
            else
            {
                side.transform.localScale = new Vector3(Math.Abs(side.transform.localScale.x) * -1, side.transform.localScale.y, side.transform.localScale.z);
            }
            

            // Set the side to active
            side.gameObject.SetActive(true);


            // Turn of the others
            front.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
        }
        else if(xInput <= -0.5f)
        {
            // Moving Left
            IsWalking = true;

            // Toggle animation
            front.SetBool("Walk", IsWalking);
            side.SetBool("Walk", IsWalking);
            back.SetBool("Walk", IsWalking);

            if(Class == "Illusionist")
            {
                side.transform.localScale = new Vector3(Math.Abs(side.transform.localScale.x) * -1, side.transform.localScale.y, side.transform.localScale.z);
            }
            else
            {
                side.transform.localScale = new Vector3(Math.Abs(side.transform.localScale.x), side.transform.localScale.y, side.transform.localScale.z);
            }
            

            // Set the side to active
            side.gameObject.SetActive(true);

            // Turn off the others
            front.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
        }
        else if (yInput >= 0.5f)
        {
            // Moving Up
            IsWalking = true;

            // Toggle animation
            front.SetBool("Walk", IsWalking);
            side.SetBool("Walk", IsWalking);
            back.SetBool("Walk", IsWalking);

            // Set the back to active
            back.gameObject.SetActive(true);

            // Turn off the others
            front.gameObject.SetActive(false);
            side.gameObject.SetActive(false);
        }
        else if (yInput <= -0.5f)
        {
            // Moving Down
            IsWalking = true;

            // Toggle animation
            front.SetBool("Walk", IsWalking);
            side.SetBool("Walk", IsWalking);
            back.SetBool("Walk", IsWalking);

            // Set the front to active
            front.gameObject.SetActive(true);

            // Turn off the others
            back.gameObject.SetActive(false);
            side.gameObject.SetActive(false);
        }
        else
        {
            IsWalking = false;

            // Toggle animation
            front.SetBool("Walk", IsWalking);
            side.SetBool("Walk", IsWalking);
            back.SetBool("Walk", IsWalking);
        }

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
            // Drop a held Item, handle UI images
            if (grabbed)
            {
                DropItem();
            }
            
        }

        // X Button Pressed
        if (Input.GetButtonDown("X_Button_" + PlayerNumber))
        {
            if(grabbed)
            {
                // Get the helditem's description
                HeldItems item = HeldItem.GetComponent<HeldItems>();
                if(item != null)
                {
                    BubbleText.text = item.ItemDescription;
                }
                else
                {
                    BubbleText.text = "Looks like I'm holding " + HeldItem.name;
                }

                SpeechBubble.SetActive(true);
                Invoke("DeactivateSpeechBubble", 4f);

                // Special case for Rusty knife that hurts the player
                if(HeldItem.name == "Rusty Knife")
                {
                    TakeDamage();
                }
            }
        }

        // A Button Pressed
        if (Input.GetButtonDown("A_Button_" + PlayerNumber) || Input.GetKeyDown(KeyCode.Space))
        {
            // try to interact
            if (Interacting)
            {
                ActivateSpeechBubble();
                
                InteractingWith.TryToInteractWithThisObject(this.Class, ref this.HeldItem, ref this.BubbleText, ref this.grabbed);

                if(grabbed)
                {
                    // Put into UI Inventory
                    HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;

                    // Turn off the item
                    HeldItem.SetActive(false);
                }
                else
                {
                    // Take out of UI Inventory
                    HeldItemImage.sprite = defaultItem;
                }
                return;
            }
            else if (StoveInteracting)
            {
                ActivateSpeechBubble();

                StoveInteractingWith.TryToInteractWithThisObject(this.Class, ref this.HeldItem, ref this.BubbleText, ref this.grabbed);

                if (grabbed)
                {
                    // Put into UI Inventory
                    HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;

                    // Turn off the item
                    HeldItem.SetActive(false);
                }
                else
                {
                    // Take out of UI Inventory
                    HeldItemImage.sprite = defaultItem;
                }
                return;
            }

            print("Throwing out raycast to look for objects to pickup");


            //Raycast Zone of pickup after shifting the pickup orgin down
            Vector3 Adjustment = new Vector2(0, 3.3f);
            Collider2D[] collide = Physics2D.OverlapBoxAll(transform.position + Adjustment + (dir * 10), new Vector2(10, 10), 0);


            bool collidedWithAnItem = false;
            string collidedWithName = null;
            // Search each collision looking for an item to pickup
            foreach (Collider2D collision in collide)
            {
                // Make sure you can actually pick item up before moving it
                if (collision.tag == "Item" && collision.gameObject != HeldItem && collision.gameObject.GetComponent<HeldItems>().CanPickThisUp)
                {
                    collidedWithAnItem = true;

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
                // Ignore players
                else if(collision.tag == "Player")
                {
                    continue;
                }

                // Save the collided with name for later to put in speech bubble
                collidedWithName = collision.name;
            }

            // Show a speech bubble of whatever else was collided with
            if(!collidedWithAnItem && collidedWithName != null)
            {
                ActivateSpeechBubble();
                BubbleText.text = "It's a " + collidedWithName;
            }
        }

        // Y Button Pressed
        if (Input.GetButtonDown("Y_Button_" + PlayerNumber))
        {
            print(this.name + " just used their ability");

            
            // Call the correct ability based on object name
            switch (Class)
            {
                case "Scholar":
                    DoScholarAbility();
                    return;
                case "Burner":
                    DoBurnerAbility();
                    return;
                case "Parkourist":
                    DoParkouristAbility();
                    return;
                case "Illusionist":
                    DoIllusionistAbility();
                    return;
            }
        }

        // If someone other than burner is holding fireball they take damage
        if (CanBeBurned && grabbed && HeldItem.name == "Fireball" && Class != "Burner")
        {
            CanBeBurned = false;
            TakeDamage();
            Invoke("CanBeBurnedAgain", 1.5f);
        }
    }

    private void CanBeBurnedAgain()
    {
        CanBeBurned = true;
    }

    /// <summary>
    /// Use this helper method to have this player take damage
    /// </summary>
    public void TakeDamage()
    {
        health--;

        // Change Health UI Meter
        HealthBar.value = health;

        // Play noise
        game_controller.PlayTakeDamageSound();

        print("Health:  " + health);
        // Check if it's time to die
        if (health <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Helper method that will heal the player for 1 HP and update the slider
    /// </summary>
    private void heal()
    {
        if (health < 5) { health++; }

        // Change Health UI Meter
        HealthBar.value = health;
    }

    /// <summary>
    /// This kills the character and puts a time for them to come back to life
    /// </summary>
    private void Die()
    {
        if (grabbed) { DropItem(); }

        Dead = true;
        this.gameObject.SetActive(false);

        // Bring back to life in 2 seconds
        Invoke("ResetCharacter", 2f);
    }

    /// <summary>
    /// Resets the health and position of this character back to where it started.
    /// </summary>
    private void ResetCharacter()
    {
        health = 5;
        HealthBar.value = health;
        Dead = false;
        transform.position = StartingPosition;
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// The burner will activate a glow. It will be the whitebox's scripts responsibility to fulfill burner requirements for events
    /// </summary>
    void DoBurnerAbility()
    {
        BurnerGlowLight.intensity = 600;
        Invoke("TurnOffBurnerGlow", 1f);

        FireParticle.Invoke();
        game_controller.PlayBurnerAbilitySound();

        // Also try to light the stove
        if (StoveInteracting)
        {
            StoveInteractingWith.LightOrUnlightTheStove();
            return;
        }
        else if(Interacting && InteractingObject.name == "Broken Boiler")
        {
            game_controller.TurnOnTheBoiler();
        }

        // Fireball optional
        if (grabbed && HeldItem.name == "Dim-Ball")
        {
            Destroy(HeldItem);
            HeldItem = GameObject.Find("Fireball");

            // Put into UI Inventory
            HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void TurnOffBurnerGlow()
    {
        BurnerGlowLight.intensity = 0;
    }

    /// <summary>
    /// The Parkourist will get a burst of speed for 1 second
    /// </summary>
    void DoParkouristAbility()
    {
        if (ParkourCanDouble)
        {
            this.SPEED *= 2;
            Invoke("TurnOffSpeedBoost", 1f);
            ParkourCanDouble = false;

            game_controller.PlayParkouristAbilitySound();
            FireParticle.Invoke();
        }
        
    }

    void TurnOffSpeedBoost()
    {
        this.SPEED /= 2;
        ParkourCanDouble = true;
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

            FireParticle.Invoke();
            game_controller.PlayScholarAbilitySound();

            switch (HeldItem.name)
            {
                case "Book Of Fruit":
                    BubbleText.text = "In the last phase of life, the plant will produce a fruit that may be more delicious to consume in this stage.";
                    return;
                case "Cookbook":
                    BubbleText.text = "Cultist Wine: Tastes much like blood and is attractive to water-dwellers. Combine Pale Tonic, Fresh Mossflower, and Pomegranate together; then warm to body temperature.";
                    return;
                case "Loose Ledger":
                    BubbleText.text = "Stored in the westernmost bookcase should be the books of seeds and sprouts, in that order. The easternmost bookcase will store the books of flowers and fruit, in that order.";
                    return;
                case "Book Of Sprouts":
                    BubbleText.text = "In the second phase of life, the seeds will grow to green, leafy sprouts.";
                    return;
                case "Book Of Flowers":
                    BubbleText.text = "In the third phase of life, the sprouts will grow to flowers. Strawberries, specifically, are white.";
                    return;
                case "Monster Manual":
                    BubbleText.text = "Curiously enough, the monsters of Lockbright enjoy music. It calms them.";
                    return;
                case "Dispensary Note":
                    BubbleText.text = "Please donate six examples of published works written in the language of our culture.";
                    return;
                case "Lusty Lore":
                    BubbleText.text = "\"I must finish my cleaning, sir. The mistress will have my head if I do not!\"\n\"Cleaning? I have something for you- polish my spear.\"\n\"But sir! It's huge! It could take all night!\"\n\"Plenty of time, my sweet. Plenty of time.\"";
                    return;
                case "Book Of Charms":
                    BubbleText.text = "A sun charm requires heat from two elements. To achieve this, douse a mystic fireball in boiled water.";
                    return;
                case "Book Of Seeds":
                    BubbleText.text = "In the first phase of life, a seed should be planted in fertile soil, watered sufficiently, and provided plenty of natural light.";
                    return;
                case "Master's Diary":
                    BubbleText.text = "The boiler, when broken, was repaired by securing it with rope, installing a new heat-bulb, fueling it with glowing energy oil, and then it may be lit for continued use.";
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
        FireParticle.Invoke();
        game_controller.PlayIllusionistAbilitySound();

        // Send out a large Raycast looking for players to heal
        Collider2D[] collide = Physics2D.OverlapBoxAll(transform.position, new Vector2(40, 40), 0);
        foreach(Collider2D collision in collide)
        {
            // If a player is found, heal them
            if(collision.gameObject.tag == "Player")
            {
                PlayerInput player = collision.gameObject.GetComponent<PlayerInput>();

                player.heal();
            }
        }

        // Heal a Dried Mossflower if held
        if (grabbed && HeldItem.name == "Dried Mossflower")
        {
            Destroy(HeldItem);
            HeldItem = GameObject.Find("Living Mossflower");

            // Put into UI Inventory
            HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;
        }

        // Restore Dim Vial
        if (grabbed && HeldItem.name == "Dim Vial")
        {
            Destroy(HeldItem);
            HeldItem = GameObject.Find("Glow Oil");

            // Put into UI Inventory
            HeldItemImage.sprite = HeldItem.GetComponent<SpriteRenderer>().sprite;
        }
    }

    

    /// <summary>
    /// This function will drop an item if there is one. This is done by dereferncing the item held
    /// </summary>
    void DropItem()
    {
        print("Dropped Item");

        HeldItem.SetActive(true);

        // Move the item near the feet
        HeldItem.transform.position = holdpoint.position + new Vector3(0,-2.5f,0);

        HeldItem = null;
        grabbed = false;

        // Take out of UI Inventory
        HeldItemImage.sprite = defaultItem;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print(this.name + " has collided with " + collision.collider.name);

        // Get the interaction object if it exists
        this.InteractingWith = collision.collider.GetComponent<Interaction>();
        this.InteractingObject = collision.collider.gameObject;
        if (InteractingWith != null)
        {
            Interacting = true;
        }
        // Or Get stove
        this.StoveInteractingWith = collision.collider.GetComponent<StoveScript>();
        if(StoveInteractingWith != null)
        {
            StoveInteracting = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        this.InteractingObject = null;
        InteractingWith = null;
        Interacting = false;
        StoveInteractingWith = null;
        StoveInteracting = false;
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
