using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Orb", menuName = "Weapon/Orb")]
public class Orb : RangedWeapon
{
    //The max distance the orb travels before it heads back
    public float max_distance;

    GameObject alive_orb;

    public override void Attack(){
        if(alive_orb != null){
            return;
        }
        base.Attack();
    }

    public override GameObject FireProjectile(){
        GameObject orb = base.FireProjectile();

        orb.GetComponent<OrbProjectile>().SetSender(holding_actor);

        float angle = orb.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        orb.GetComponent<OrbProjectile>().max_distance = this.max_distance;
        orb.GetComponent<OrbProjectile>().SetInitialVelocity(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

        alive_orb = orb;

        return orb;
    }
}
