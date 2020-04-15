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
        Front_Animator.gameObject.SetActive(true);
        Back_Animator.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            print("Monster hurt a player!!");

            PlayerInput player = collision.collider.GetComponent<PlayerInput>();

            player.TakeDamage();

            
        }
    }
}
