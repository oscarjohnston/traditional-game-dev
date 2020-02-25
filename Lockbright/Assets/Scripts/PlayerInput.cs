using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    private float xInput, yInput;
    public float SPEED = 1.0f;

    public int PlayerNumber = 1;

    public GameObject SpeechBubble;
    public Text BubbleText;
    private bool Interactable;

    private Rigidbody2D Body;

    private bool AbleToChangePlayerNumber;
    private float INTERACTABLE_TIME = 1f;

    //Grabber Code
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 5f;
    public Transform holdpoint;

    Vector3 previousGood = Vector3.zero;

    public GameObject HeldItem;

    public UnityEvent InteractWithStove;
    public UnityEvent InteractWithFridge;
    public UnityEvent WinGame;

    // Start is called before the first frame update
    void Start()
    {
        Body = GetComponent<Rigidbody2D>();

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
                //So you don't pick yourself up
                Physics2D.queriesStartInColliders = false;

                //Raycast Zone of pickup
                hit = Physics2D.Raycast(transform.position, dir * transform.localScale.x, distance);
                Debug.DrawRay(transform.position, dir, Color.green);

                //Makes sure you can actually pick item up before moving it
                if (hit.collider != null && hit.collider.tag == "Item")
                {
                    grabbed = true;
                    HeldItem = hit.collider.gameObject;
                }
            }
            else if(!Physics2D.OverlapPoint(holdpoint.position))
            {
                grabbed = false;
                HeldItem = null;
            }
        }

        //Moves item to holdpoint
        if (grabbed)
        {
            hit.collider.gameObject.transform.position = holdpoint.position;
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
