using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public int damage;

    // Animators
    public Animator Front_Animator;
    public Animator Back_Animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            player.health -= 1;
        }
    }
}
