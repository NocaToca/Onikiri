using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Arrow : Projectile
{
    //Empty atm bc almost everything was moved to the projectile class
   
   protected override void OnTriggerEnter2D(Collider2D other){
        //We have to make sure we aren't colliding with another arrow as well
        //We'll make a projectile tag for anything else later but for now we have this
        Arrow possible_arrow = other.gameObject.GetComponent<Arrow>();
        if(possible_arrow != null){
            return;
        }

        if(other.gameObject == sender){
            return;
        }

        base.OnTriggerEnter2D(other);
        Destroy(this.gameObject);
   }
}
