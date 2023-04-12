using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword", menuName = "Weapon/Sword")]
public class Sword : MeleeWeapon
{
    public override void Attack(){
        PlayAttackAnimation();
        if(holding_actor is Player){
            Player p = (Player)holding_actor;
            p.sword_collider.ApplyToEnemies(OnHit);
            //Debug.Log("Sent");
        }
    }

    public override void Attack(float damage_multiplier){
        PlayAttackAnimation();
        if(holding_actor is Player){
            Player p = (Player)holding_actor;
            EmpowerNextHit(damage_multiplier);
            p.sword_collider.ApplyToEnemies(OnHit);
            //Debug.Log("Sent");
        }
    }
}
