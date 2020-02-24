using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public bool grabbed;
    RaycastHit2D hit;
    public float distance = 5f;
    public Transform holdpoint;

    Vector3 previousGood = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 dir = new Vector2(x, y);
        if (dir == Vector3.zero)
        {
            dir = previousGood;
        }
        else
        {
            previousGood = dir;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!grabbed)
            {

                //So you don't pick yourself up
                Physics2D.queriesStartInColliders = false;

                //Raycast Zone of pickup
                hit = Physics2D.Raycast(transform.position, dir * transform.localScale.x, distance);
                Debug.DrawRay(transform.position, dir, Color.green);

                //Makes sure you can actually pick item up before moving it
                if(hit.collider != null && hit.collider.tag == "Item")
                {
                    grabbed = true;
                }

            }
        }

        //Moves item to holdpoint
        if (grabbed)
        {
            hit.collider.gameObject.transform.position = holdpoint.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * transform.localScale.x * distance);
    }
}
