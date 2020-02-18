using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float SPEED = 10.0f;

    public string PlayerNumber = "1";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float VerticalTranslation = Input.GetAxis("Vertical" +  PlayerNumber) * SPEED;
        float HorizontalTranslation = Input.GetAxis("Horizontal" + PlayerNumber) * SPEED;

        VerticalTranslation *= Time.deltaTime;
        HorizontalTranslation *= Time.deltaTime;

        transform.Translate(HorizontalTranslation, VerticalTranslation, 0);
    }
}
