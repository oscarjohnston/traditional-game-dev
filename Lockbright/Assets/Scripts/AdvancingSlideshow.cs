using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdvancingSlideshow : MonoBehaviour
{
    public GameObject[] slides;

    private int CurrentSlide;
    private bool SlideshowIsActive = false;
    private bool ButtonIsPressable;

    // Start is called before the first frame update
    void Start()
    {
        ButtonIsPressable = false;
        SlideshowIsActive = false;
        CurrentSlide = 0;
        SetCurrentSlideToShow();
    }

    // Update is called once per frame
    void Update()
    {
        if (!ButtonIsPressable || !SlideshowIsActive) { return; }

        if(Input.GetButtonDown("A_Button_1") || Input.GetButtonDown("A_Button_2") || Input.GetButtonDown("A_Button_3") || Input.GetButtonDown("A_Button_4"))
        {
            ButtonIsPressable = false;
            AdvanceSlideshow();
            Invoke("MakeButtonPressable", 2.0f);
        }
    }

    private void MakeButtonPressable()
    {
        ButtonIsPressable = true;
    }

    private void AdvanceSlideshow()
    {
        // Kill slide show after 
        if(CurrentSlide == slides.Length - 1)
        {
            this.gameObject.SetActive(false);

            // Go to main game scene
            SceneManager.LoadScene("Game");
        }

        CurrentSlide++;
        SetCurrentSlideToShow();
    }

    private void SetCurrentSlideToShow()
    {
        for(int i = 0; i < slides.Length; i++)
        {
            if(i == CurrentSlide)
            {
                slides[i].SetActive(true);
            }
            else
            {
                slides[i].SetActive(false);
            }
        }
    }

    public void ActivateSlideshow()
    {
        SlideshowIsActive = true;
        Invoke("MakeButtonPressable", 2.0f);
    }
}
