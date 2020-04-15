using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FridgeMonster : MonoBehaviour
{
    public Transform destination;
    private bool Move = false;
    private Vector3 difference;
    private float MoveSpeed = 100;
    private int MoveCounter;

    // Start is called before the first frame update
    void Start()
    {
        MoveCounter = 0;
        difference = (destination.position - transform.position) / MoveSpeed;
        difference = new Vector3(difference.x, difference.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Move && MoveCounter <= MoveSpeed)
        {
            transform.position += difference;
            MoveCounter++;
        }
    }

    public void SpawnMonster()
    {
        this.gameObject.SetActive(true);
        Move = true;
    }
}
