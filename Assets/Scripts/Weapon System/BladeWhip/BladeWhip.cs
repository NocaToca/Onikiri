using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blade and Whip", menuName = "Weapon/Blade and Whip")]
//The blade and whip is kinda a special case, so we're making it an independent weapon
public class BladeWhip : Weapon
{
    public GameObject whip_prefab;
    public float whip_length;

    public float damage;

    private Whip whip;

    public override void Attack(){
        base.Attack();

        if(whip == null){
            GameObject.Instantiate(whip_prefab);
            whip_prefab.transform.SetParent(holding_actor.gameObject.transform);
        }

        whip.Attack(damage);

    }


}
