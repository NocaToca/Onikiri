using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boon : Augment
{

    public bool already_activated;

    void Start(){
        already_activated = false;
    }
    
    //Updates stats and adds related update function
    public virtual void ActivateBoon(Player p){
        if(already_activated){
            return;
        }
        already_activated = true;
        p.update_boons.AddListener(UpdateSubscriber);
    }

    public virtual void UpdateSubscriber(){

    }


}
