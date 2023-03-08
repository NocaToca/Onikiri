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

        return SUCCESS();
        
    }


}
