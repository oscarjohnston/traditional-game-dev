using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        var moveVector = new Vector3(xInput, yInput, 0) * SPEED * Time.deltaTime;

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

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        print("Player " + PlayerNumber + " has collided with " + collision.collider.name);

        //ToggleSpeechBubble(true);
       // BubbleText.text = "Life is pain";
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
            BubbleText.text = "Wow! It's a " + collision.collider.name;
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
