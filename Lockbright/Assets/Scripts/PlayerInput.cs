using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    public float SPEED = 1.0f;

    public string PlayerNumber = "1";

    public Image SpeechBubble;
    public Text BubbleText;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        ToggleSpeechBubble(false);
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalTranslation = Input.GetAxis("Vertical" +  PlayerNumber) * SPEED;
        float HorizontalTranslation = Input.GetAxis("Horizontal" + PlayerNumber) * SPEED;

        VerticalTranslation *= Time.deltaTime;
        HorizontalTranslation *= Time.deltaTime;

        //transform.Translate(HorizontalTranslation, VerticalTranslation, 0);
        //rb2d.MovePosition(new Vector2(HorizontalTranslation, VerticalTranslation));
        rb2d.velocity = new Vector2(HorizontalTranslation, VerticalTranslation);
    }

    /// <summary>
    /// Toggles the speech bubble that goes over the player.
    /// </summary>
    /// <param name="changeTo">The state you want the bubble to be</param>
    private void ToggleSpeechBubble(bool changeTo)
    {
        SpeechBubble.enabled = changeTo;
        BubbleText.enabled = changeTo;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        print("Colliding!");

        ToggleSpeechBubble(true);
        BubbleText.text = "Life is pain";
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ToggleSpeechBubble(false);
    }
    
}
