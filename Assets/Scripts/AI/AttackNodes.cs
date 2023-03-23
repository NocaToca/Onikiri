using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

//What weapon we have
public enum AttackMode{
    Ranged,
    Melee
}

//Basic attack for AI
public class AttackNode : AINode
{
    //Weapon or ranged attack
    AttackMode mode;

    //The responsible actor
    Actor a;

    //Their weapon
    Weapon weapon;

    //If our weapon is a ranged weapon, our attack mode is obviously ranged. The converse is also true
    public AttackNode(Actor a, Weapon weapon) : base(){
        if(weapon is RangedWeapon){
            mode = AttackMode.Ranged;
        } else {
            mode = AttackMode.Melee;
        }

        this.a = a;
        this.weapon = weapon;
    }

    //We're going to need a target to attack
    public override Status Evaluate(){
        object target = GetData("target");

        if(target == null){
            return FAIL();
        }

        //For now we are assuming our target is the player, but it doesn't have to be if we edit it
        GameObject player = (GameObject)target;

        //Melee attack (Ranged attack not implemented)
        if(mode == AttackMode.Melee){

            //Is our melee weapon's hitbox overlapping?
            MeleeWeapon melee_weapon = (MeleeWeapon)weapon;
            Vector2 point = new Vector2(a.transform.position.x, a.transform.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, melee_weapon.attacking_distance);

            // Debug.Log(colliders.Length);

            foreach(Collider2D col in colliders){
                if(col.gameObject.tag == "Player"){
                    return SUCCESS();
                }
            }

            return FAIL();
        }

        return SUCCESS();
        
    }
}

//Our attack action
public class Attack : AINode{

    Actor a;
    Weapon weapon;

    //Internal float to measure time 
    private float time_spent_waiting;

    public Attack(Actor a, Weapon weapon) : base(){
        this.a = a;
        this.weapon = weapon;
    }

    public override Status Evaluate(){
        Enemy enemy_class = (Enemy)a;

        //Attack at attack speed intervals
        if(time_spent_waiting >= enemy_class.attack_speed){
            GameObject player = (GameObject)GetData("target");
            weapon.Attack(player);
            time_spent_waiting = 0.0f;
        }

        time_spent_waiting += Game.tick;

        return RUNNING();
    }

}
