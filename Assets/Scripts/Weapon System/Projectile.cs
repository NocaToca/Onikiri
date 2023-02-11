using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float base_speed;

    [HideInInspector]
    public Actor sender; //Made once a arrow is instantiated 
    protected Rigidbody2D rb;

    protected BoxCollider2D bc;

    public void SetSender(Actor actor){
        sender = actor;
    }

    public void SetSender(GameObject sender){
        this.sender = sender.GetComponent<Actor>();
    }

    public void SetInitialVelocity(Vector2 angle){
        rb.velocity = angle * base_speed;
    }

    protected virtual void Awake(){
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(sender == null){
            Debug.LogWarning("Arrow has no object of origin. Creation might be an error");
        }
    }

    //Collision
    protected virtual void OnTriggerEnter2D(Collider2D other) {

        

        

        Actor possible_actor = Actor.ExtractActor(other.gameObject);
        if(possible_actor is Enemy){
            possible_actor.TakeDamage(10.0f);
        }

        
    }
}
