using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction2 : MonoBehaviour
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
        PlayerInput test = collision.collider.GetComponent<PlayerInput>();

        if (test != null)
        {
            GameObject item = test.HeldItem;
        }

        /*
        if (collision.gameObject.GetComponent("HeldItem").gameObject.name == "Cube" && Input.GetKeyDown(KeyCode.E) && !spawned)
        {
            print("Interacted with stove, spawning key");
            Instantiate(reward, spawnPoint, true);
            spawned = true;
        }
        */
    }
}
