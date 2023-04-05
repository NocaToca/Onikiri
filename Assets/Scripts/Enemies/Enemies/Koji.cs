using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koji : Enemy
{
    public Bow fireball;

    protected override void Start(){
        base.Start();
        fireball.holding_actor = this;
    }

    public void ShootFireball(){
        fireball.FireProjectile(GameObject.FindGameObjectWithTag("Player").transform.position);
    }
}
