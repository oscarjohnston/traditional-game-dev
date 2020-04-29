using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Sound Effects for game
    public UnityEvent StartBackgroundMusic;
    public UnityEvent ParkouristAbilitySound;
    public UnityEvent BurnerAbilitySound;
    public UnityEvent ScholarAbilitySound;
    public UnityEvent IllusionistAbilitySound;
    public UnityEvent TakeDamageSound;
    public UnityEvent ChangeCharacters;

    public float TimeLeft;
    public Text TimerText;
    public Text GameOverText;

    // Bookshelf Puzzle
    private int BookshelfCounter = 3;
    private static int DustyRecordCounter = 4;
    public GameObject DustyRecord;
    // Bookshelf Icons
    public GameObject BookOfSeeds;
    public GameObject BookOfSprouts;
    public GameObject BookOfFlowers;
    public GameObject BookOfFruit;

    // Boiler Puzzle
    private bool BoilerIsOn = false;
    private bool CanBoilerBeTurnedOn = false;
    public Sprite RepairedBoiler;
    public GameObject Boiler;
    public GameObject BoilerLight;

    // Ladder System
    public GameObject StudyLadder;
    public GameObject LoungeLadder;

    // Moon Brick Puzzle Visuals
    public GameObject HalfMoonBrick;
    public GameObject CrescentMoonBrick;

    // Open Trunk Charm Visuals
    public GameObject EastCharm;
    public GameObject WestCharm;

    // Start is called before the first frame update
    void Start()
    {
        // On start, play the background music
        StartBackgroundMusic.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        TimeLeft -= Time.deltaTime;
        if(TimeLeft < 0)
        {
            GameOverText.gameObject.SetActive(true);
            Invoke("SendToMainMenu", 3f);
        }
        else
        {
            TimerText.text = "" + ((int)TimeLeft / 60) + ":" + ((int)TimeLeft % 60);
        }
    }

    void SendToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }



    public void PlayParkouristAbilitySound()
    {
        ParkouristAbilitySound.Invoke();
    }

    public void PlayBurnerAbilitySound()
    {
        BurnerAbilitySound.Invoke();
    }

    public void PlayScholarAbilitySound()
    {
        ScholarAbilitySound.Invoke();
    }

    public void PlayIllusionistAbilitySound()
    {
        IllusionistAbilitySound.Invoke();
    }

    /// <summary>
    /// This event will win the game
    /// </summary>
    public void InvokeWinGameEvent()
    {
        // Go to main game scene
        SceneManager.LoadScene("Epilogue");
    }

    public void PlayTakeDamageSound()
    {
        TakeDamageSound.Invoke();
    }

    public void DecrementBookshelfCounter()
    {
        BookshelfCounter--;

    }

    public void DecrementDustyRecordCounter()
    {
        DustyRecordCounter--;

        print("Dusty Record Counter is now:  " + DustyRecordCounter);

        // If the requirements fulfilled, spawn the Dusty Record
        if (DustyRecordCounter == 0)
        {
            print("Moving Dusty Record into position");
            DustyRecord.transform.position = new Vector3(140, 11 ,0);
            DustyRecord.GetComponent<HeldItems>().CanPickThisUp = true;
        }
    }

    public void TurnOnTheBoiler()
    {
        if (CanBoilerBeTurnedOn)
        {
            print("Boiler Has been turned on successfully");

            // Swap out the boiler image
            Boiler.GetComponent<SpriteRenderer>().sprite = RepairedBoiler;
            BoilerIsOn = true;
            BoilerLight.SetActive(true);
        }
    }

    public void CanTurnTheBoilerOn()
    {
        print("Boiler can now be turned on");
        CanBoilerBeTurnedOn = true;
    }

    public bool IsBoilerOn()
    {
        return BoilerIsOn;
    }

    public void SinkTurnOnBoilerOverride()
    {
        print("Boiler Has been turned on successfully");

        // Swap out the boiler image
        Boiler.GetComponent<SpriteRenderer>().sprite = RepairedBoiler;
        BoilerIsOn = true;
        BoilerLight.SetActive(true);
    }

    public void ActivateLadderSystem()
    {
        StudyLadder.SetActive(true);
        LoungeLadder.SetActive(true);
    }

    public void ActivateHalfMoonBrick()
    {
        HalfMoonBrick.SetActive(true);
    }

    public void ActivateCrescentMoonBrick()
    {
        CrescentMoonBrick.SetActive(true);
    }

    public void ActivateWestCharm()
    {
        WestCharm.SetActive(true);
    }

    public void ActivateEastCharm()
    {
        EastCharm.SetActive(true);
    }

    public void PlayChangeCharacterEvent()
    {
        ChangeCharacters.Invoke();
    }

    public void ActivateSeeds()
    {
        BookOfSeeds.SetActive(true);
    }

    public void ActivateSprouts()
    {
        BookOfSprouts.SetActive(true);
    }

    public void ActivateFlowers()
    {
        BookOfFlowers.SetActive(true);
    }

    public void ActivateFruit()
    {
        BookOfFruit.SetActive(true);
    }
}
