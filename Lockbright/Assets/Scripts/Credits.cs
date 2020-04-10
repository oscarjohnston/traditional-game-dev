using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private bool ButtonIsPressable;

    public Button PlayButton;

    // Start is called before the first frame update
    void Start()
    {
        ButtonIsPressable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ButtonIsPressable) { return; }

        if (Input.GetButtonDown("A_Button_1") || Input.GetButtonDown("A_Button_2") || Input.GetButtonDown("A_Button_3") || Input.GetButtonDown("A_Button_4"))
        {
            ButtonIsPressable = false;
            DeactivateCredits();
        }
    }

    private void MakeButtonPressable()
    {
        ButtonIsPressable = true;
    }

    public void ActivateCredits()
    {
        this.gameObject.SetActive(true);
        ButtonIsPressable = false;
        Invoke("MakeButtonPressable", 2.0f);
        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    private void DeactivateCredits()
    {
        this.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(PlayButton.gameObject);
    }
}
