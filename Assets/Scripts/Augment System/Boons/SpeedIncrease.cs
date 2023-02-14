using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedIncrease : Boon
{
    public override void ActivateBoon(Player p){
        base.ActivateBoon(p);
        p.speed += 1.0f;
    }
}
