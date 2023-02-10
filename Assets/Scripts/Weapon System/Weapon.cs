using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract parent class to be used with different weapon types

public abstract class Weapon : ScriptableObject{
    
    [HideInInspector]
    public Actor holding_actor;


    public virtual void Attack(){
        //plays attacking animation and behavoir
        PlayAttackAnimation();
    }

    //Plays whatever animination we have stored for our attack
    public virtual void PlayAttackAnimation(){

    }

    public virtual void OnHit(){

    }
    
}
