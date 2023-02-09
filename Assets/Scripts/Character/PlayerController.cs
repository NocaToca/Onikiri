using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    Controls all of the movement regarding the player
    Basically is the buffer parsing input before sending it to the player object

    That is why the player holds most environment variables and not this class, regardless whether it is based off of movement
*/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Player p;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        p = GetComponent<Player>();
        if(p == null){
            Debug.LogError("Player Controller is attached to a game object without a player component");
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement(){

        Vector2 velocity = Vector2.zero;

        //North
        if(Input.GetKey(KeyCode.W)){
            velocity.y += p.speed;
        }
        //South
        if(Input.GetKey(KeyCode.S)){
            velocity.y -= p.speed;
        }
        //West
        if(Input.GetKey(KeyCode.A)){
            velocity.x -= p.speed;
        }
        //East
        if(Input.GetKey(KeyCode.D)){
            velocity.x += p.speed;
        }

        velocity = velocity.normalized;

        if(Input.GetKey(KeyCode.LeftShift)){
            velocity *= p.boost_speed;
        }

        rb.velocity = velocity;

    }
}
