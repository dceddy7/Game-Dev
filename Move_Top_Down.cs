using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Top_Down : MonoBehaviour
{
    private Rigidbody2D rb2d;
    GameObject player;
    public GameObject playerBody;
    float angle;
    public float speed = 5f;
    private Vector3 point = Vector3.zero;
    public float turnSpeed = 20f;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = this.gameObject;

    }


    void Update()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        //rb2d.velocity = new Vector2(x * speed , y * speed );
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward, turnSpeed * Time.deltaTime);

        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward, turnSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)) { 
            if (Input.GetKey(KeyCode.W))
            {
                rb2d.velocity = transform.right * speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                rb2d.velocity = transform.right * -speed;
            }
        }
        else
        {
            rb2d.velocity = Vector2.zero;
        }



     }
}
