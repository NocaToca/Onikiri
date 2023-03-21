using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blade and Whip", menuName = "Weapon/Blade and Whip")]
//The blade and whip is kinda a special case, so we're making it an independent weapon
public class BladeWhip : Weapon
{
    public GameObject projectile_prefab;
    public float whip_length;

    public override void Attack(){
        base.Attack();

        GameObject whip = Instantiate(projectile_prefab, holding_actor.transform.position + (new Vector3(0, 1.0f, 0)), Quaternion.identity);
        whip.transform.parent = holding_actor.transform;
        whip.GetComponent<Whip>().whip_length = whip_length;
    }


}
