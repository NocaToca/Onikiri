using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeWeapon : Weapon
{
    public float attacking_distance;
    public float damage;

    public override void Attack(){
        base.Attack();

        RaycastHit2D hit;

        if(ScanInFront(holding_actor, out hit)){
            //Then we check if the cast hit an actor
            Actor a = Actor.ExtractActor(hit.collider.gameObject);
            if(a != null){
                OnHit(a);
            }
        }
    }

    public override void Attack(GameObject inflicting_actor){
        OnHit(inflicting_actor.GetComponent<Actor>());
    }


    public virtual bool ScanInFront(Actor actor, out RaycastHit2D hit){
        //Essentially we're going to scan in front of the actor to see if there are any enemies in front of them. We will probably change the direction later but for now we're going 
        //To use the mouse for ease of comfort

        Vector3 looking_direction = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 cast_direction = looking_direction - actor.transform.position;

        Vector2 origin = new Vector2(actor.transform.position.x, actor.transform.position.y);
        Vector2 direction = new Vector2(cast_direction.x, cast_direction.y);

        int layer = 1 << 7;

        hit = Physics2D.Raycast(origin, direction, attacking_distance, layer);

        //Debug.Log(hit.collider != null);

        return hit.collider != null;

    }

    protected float damage_mul = 1.0f;

    protected void EmpowerNextHit(float damage){
        damage_mul = damage;
    }

    public virtual void OnHit(Actor a){
        a.TakeDamage(damage * damage_mul);
        damage_mul = 1.0f;
    }
}
