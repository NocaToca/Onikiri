using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The whips attack is basically long ranged melee with an AOE collider shifted in. For everything, we're going to use colliders
public class Whip : MonoBehaviour
{
    
    List<WhipCollider> colliders;

    public int collider_index;

    void Start(){
        collider_index = 0;
        if(colliders == null){
            Debug.LogWarning("No colliders on the whip object, will be broken");
        }
    }

    public void Attack(float whip_damage){
        WhipCollider current_collider = colliders[collider_index%colliders.Count];
        if(current_collider.enemies_in_collider){
            foreach(GameObject obj in current_collider.enemies){
                Actor enemy = Actor.ExtractActor(obj);
                enemy.TakeDamage(whip_damage);
            }
        }

        collider_index++;
    }

}
