using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bow", menuName = "Weapon/Bow")]
public class Bow : Weapon
{

    public GameObject arrow_prefab;

    public override void Attack(){
        base.Attack(); //Calls base attack function to do default actions
        FireProjectile();
    }

    //Fires our projectile stored in our bow
    public void FireProjectile(){

    }
}
