using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    public float detectionRadius;
    private bool detectedPlayer = false;
    private GameObject target;
    private LayerMask mask = ~(1 << 10);
    public float searchTime = 10;
    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckState();
    }


    public void CheckState()
    {
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green);
        RaycastHit2D foundPlayer = Physics2D.Raycast(transform.position, target.transform.position - transform.position, Mathf.Infinity, mask);

        print(foundPlayer.distance);
        if ((foundPlayer.collider.gameObject.tag == "Player") && (foundPlayer.distance < detectionRadius))
        {
            detectedPlayer = true;
        }

        /*
        if (Vector2.Distance(target.transform.position, transform.position) < 10)
        {
            detectedPlayer = true;
            print("CAUGHT YOU");
        } 
        */

        if(detectedPlayer == true)
        {
            
            
           // Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, target.transform.position - transform.position, Mathf.Infinity, mask);

            if(hit.collider.gameObject.tag == "Player")
            {
                //reset the countdown
                timeLeft = searchTime;
                transform.right = target.transform.position - transform.position;
                //print("i see you");
            }
            else
            {
                timeLeft -= Time.deltaTime;
                print("i dont see you");
            }

            if(timeLeft < 0)
            {
                detectedPlayer = false;
                print("i lost you");

            }

            //print(hit.collider.name);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
