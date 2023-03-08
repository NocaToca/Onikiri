using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//abstract parent class to be used with different weapon types

public abstract class Weapon : ScriptableObject{
    
    [HideInInspector]
    public Actor holding_actor;

    public Enchantment enchantment;


    public virtual void Attack(){
        //plays attacking animation and behavoir
        PlayAttackAnimation();
    }

    public virtual void Attack(GameObject inflicting_actor){
        Attack();
    }

    //Plays whatever animination we have stored for our attack
    public virtual void PlayAttackAnimation(){

    }

    
    
}
