using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Augments;


/*
    Controls all of the movement regarding the player
    Basically is the buffer parsing input before sending it to the player object

    That is why the player holds most environment variables and not this class, regardless whether it is based off of movement
*/
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : Controller
{
    private Player p;
    private Rigidbody2D rb;

    // List<SkillListener> skills;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        
        p = GetComponent<Player>();
        if(p == null){
            Debug.LogError("Player Controller is attached to a game object without a player component");
        }

        rb = GetComponent<Rigidbody2D>();
        accepting_movement = true;
    }

    void Update(){
        HandleInteractionInput();
    }

    void HandleInteractionInput(){
        if(Input.GetMouseButtonDown(0)){
            p.AttemptAttack();
        }
        if(Input.GetKeyDown(KeyCode.T)){
            p.SwapWeapons();
        }
        if(Input.GetKeyDown(KeyCode.R)){
            p.Respawn();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = Vector2.zero;
        HandleMovement(1);
    }

    private void HandleMovement(float deltaTime){
        if(!accepting_movement){
            return;
        }

        Vector2 velocity = Vector2.zero;

        //Northhow to tell if no keys are being pressed unity
        if(Input.GetKey(KeyCode.W)){
            velocity.y += p.speed * deltaTime;
        }
        //South
        if(Input.GetKey(KeyCode.S)){
            velocity.y -= p.speed * deltaTime;
        }
        //West
        if(Input.GetKey(KeyCode.A)){
            velocity.x -= p.speed * deltaTime;
        }
        //East
        if(Input.GetKey(KeyCode.D)){
            velocity.x += p.speed * deltaTime;
        }

        velocity = velocity.normalized;
        //Debug.Log(velocity);

        if(Input.GetKey(KeyCode.LeftShift)){
            velocity.y *= p.boost_speed;
            velocity.x *= p.boost_speed;
        }
        if(velocity != Vector2.zero){
            p.movedLastFrame = true;
        } else {
            p.movedLastFrame = false;
        }

        rb.velocity = velocity;

    }
}

//Structure to add a specific listener to the player, used for skills
// public class SkillListener{

//     List<WeaponPass> weapons;
//     KeyCode key;
//     UnityEvent event;

//     public SkillListener(List<WeaponPass> weapons, KeyCode key, UnityEvent event){
//         this.weapons = weapons;
//         this.key = key;
//         this.event = event;
//     }

// }
