using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : MonoBehaviour
{

    public float base_speed;

    [HideInInspector]
    public Actor sender; //Made once a arrow is instantiated 
    private Rigidbody2D rb;

    private BoxCollider2D bc;

    public void SetSender(Actor actor){
        sender = actor;
    }

    public void SetSender(GameObject sender){
        this.sender = sender.GetComponent<Actor>();
    }

    public void SetInitialVelocity(Vector2 angle){
        rb.velocity = angle * base_speed;
    }

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(sender == null){
            Debug.LogWarning("Arrow has no object of origin. Creation might be an error");
        }
    }

    //Collision
    private void OnTriggerEnter2D(Collider2D other) {

        if(other.gameObject == sender){
            return;
        }

        //We have to make sure we aren't colliding with another arrow as well
        //We'll make a projectile tag for anything else later but for now we have this
        Arrow possible_arrow = other.gameObject.GetComponent<Arrow>();
        if(possible_arrow != null){
            return;
        }

        Actor possible_actor = Actor.ExtractActor(other.gameObject);
        if(possible_actor is Enemy){
            possible_actor.TakeDamage(10.0f);
        }

        Destroy(this.gameObject);
    }

    //We want to do physics!
   
}
