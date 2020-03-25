using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    // Sound Effects for game
    public UnityEvent StartBackgroundMusic;
    public UnityEvent ParkouristAbilitySound;
    public UnityEvent BurnerAbilitySound;
    public UnityEvent ScholarAbilitySound;
    public UnityEvent IllusionistAbilitySound;

    // Game Events
    public UnityEvent WinGame;

    // Start is called before the first frame update
    void Start()
    {
        // On start, play the background music
        StartBackgroundMusic.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        WinGame.Invoke();
    }
}
