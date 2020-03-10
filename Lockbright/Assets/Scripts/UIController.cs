using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public UnityEvent ShowAbilityUsedParticle;
    public GameObject UsedAbilityParticles;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireOffUsedAbilityParticles(Vector3 position)
    {
        UsedAbilityParticles.transform.position = position;
        ShowAbilityUsedParticle.Invoke();
    }
}
