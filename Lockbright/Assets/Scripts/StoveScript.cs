using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour
{
    // Controller for particle
    public UIController game;

    // Bollean for identifying if the stove is on
    private bool StoveIsLit;

    void Start()
    {
        StoveIsLit = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Try to turn the stove on if it's not lit yet
        if (!StoveIsLit)
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            // If the burner presses Y, light the stove
            if (player != null && Input.GetButtonDown("Y_Button_3") && player.PlayerNumber == 3)
            {
                StoveIsLit = true;
                game.FireTurnOnStove();

                print("Turning on stove");
            }
        }

        // Otherwise turn it off
        else
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            // If the burner presses Y, light the stove
            if (player != null && Input.GetButtonDown("Y_Button_3") && player.PlayerNumber == 3)
            {
                StoveIsLit = false;
                game.FireTurnOffStove();

                print("Turning off stove");
            }
        }
    }
}
