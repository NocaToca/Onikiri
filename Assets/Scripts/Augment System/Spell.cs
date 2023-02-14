using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//Might actually change the augment classes to accomindate the vast differences
//We're going to attach spell componenets to the player once we get them in order to give the desired effect
public abstract class Spell : Augment
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnActivate(){
        
    }
}
