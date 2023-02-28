using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
//The basic idea is that it's sent out and comes back to the sender
public class OrbProjectile : Projectile
{

    [HideInInspector]
    public float max_distance;

    private float time_before_return;

    public float speed_scale;

    bool returning;

    Vector3 destination;

    protected override void Awake(){
        base.Awake();
        returning = false;
    }

    protected override void Start(){
        base.Start();

        time_before_return = 2.0f;

        //Our destination will be our max distance plus one of our init velocity vector
        Vector2 init_velocity = rb.velocity;

        destination = new Vector3(init_velocity.x * (max_distance+1) + this.gameObject.transform.position.x, init_velocity.y *(max_distance+1) + this.gameObject.transform.position.y, 0.0f);
    }

    protected void FixedUpdate(){

        //Debug.Log(Vector3.Distance(this.transform.position, sender.transform.position));
        // Debug.Log(returning);

        time_before_return -= Time.deltaTime;
        
        if(!returning && ((Vector3.Distance(this.transform.position, sender.transform.position) >= max_distance) || (time_before_return <= 0.0f))){
            returning = true;
        }

        Move();
    }

    void Move(){
        destination.z = 0.0f;
        Vector3 base_vel = (returning) ? sender.transform.position - transform.position : destination - transform.position;
        base_vel = base_vel.normalized;
        float speed = Mathf.Abs(base_speed - (Mathf.Abs(max_distance - Vector3.Distance(this.transform.position, destination))/max_distance));
        Debug.Log(speed);
        rb.velocity = base_vel * speed * speed_scale;
    }

    protected override void OnTriggerEnter2D(Collider2D other){
        base.OnTriggerEnter2D(other);
        
        if(returning){
            if(other.gameObject == sender.gameObject){
                Debug.Log("Destory");
                Destroy(this.gameObject);
            }
        }
   }
}
