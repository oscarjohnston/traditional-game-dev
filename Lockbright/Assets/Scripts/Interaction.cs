using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject reward;
    public Transform spawnPoint;
    public bool spawned;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /*
        if (collision.gameObject.tag == "Player" && Input.GetKeyDown(KeyCode.E) && !spawned)
        {
            print("Interacted with fridge, spawning book");
            Instantiate(reward, spawnPoint, true);
            spawned = true;
        }
        */
    }
}
