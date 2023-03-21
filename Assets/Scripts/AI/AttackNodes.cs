using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public enum AttackMode{
    Ranged,
    Melee
}

public class AttackNode : AINode
{
    AttackMode mode;

    Actor a;

    Weapon weapon;

    public AttackNode(Actor a, Weapon weapon) : base(){
        if(weapon is RangedWeapon){
            mode = AttackMode.Ranged;
        } else {
            mode = AttackMode.Melee;
        }

        this.a = a;
        this.weapon = weapon;
    }

    public override Status Evaluate(){
        object target = GetData("target");

        if(target == null){
            return FAIL();
        }

        GameObject player = (GameObject)target;
        if(mode == AttackMode.Melee){
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

public class Attack : AINode{

    Actor a;
    Weapon weapon;

    private float time_spent_waiting;

    public Attack(Actor a, Weapon weapon) : base(){
        this.a = a;
        this.weapon = weapon;
    }

    public override Status Evaluate(){
        Enemy enemy_class = (Enemy)a;

        if(time_spent_waiting >= enemy_class.attack_speed){
            GameObject player = (GameObject)GetData("target");
            weapon.Attack(player);
            time_spent_waiting = 0.0f;
        }

        time_spent_waiting += Game.tick;

        return RUNNING();
    }

}
