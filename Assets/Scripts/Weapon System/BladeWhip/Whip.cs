using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The whip visual and interaction of the blade and whip weapon
//This is basically the projectile
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Whip : MonoBehaviour
{
    public float rotational_speed;
    public float whip_length;

    Rigidbody2D rb;
    CircleCollider2D cc;

    //float angle = 90.0f;

    void Awake(){
        cc = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Actor a = Actor.ExtractActor(other.gameObject);
        if(a.gameObject.transform != this.transform.parent){
            a.TakeDamage(10.0f);
        }
    }

    //We're going to want to rotate around the fixed point
    private void FixedUpdate() {
        
        transform.RotateAround(this.transform.parent.position, new Vector3(0,0,1), rotational_speed);
        
    }
}
