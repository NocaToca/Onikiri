using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthIncrease : Boon
{
    public override void ActivateBoon(Player p){
        base.ActivateBoon(p);
        p.stats.AddHealth(50.0f);
    }
}
