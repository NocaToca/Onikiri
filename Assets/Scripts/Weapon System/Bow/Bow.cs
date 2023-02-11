using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Weapon/Bow")]
public class Bow : RangedWeapon
{

    //public GameObject arrow_prefab;

    public override GameObject FireProjectile(){
        GameObject new_arrow = base.FireProjectile();

        new_arrow.GetComponent<Arrow>().SetSender(holding_actor);

        float angle = new_arrow.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;

        new_arrow.GetComponent<Arrow>().SetInitialVelocity(new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));
        return new_arrow;
    }
}
