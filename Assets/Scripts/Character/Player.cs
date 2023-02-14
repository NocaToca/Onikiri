using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
    Handles all logic regarding the player

    For input look at PlayerController
*/
[RequireComponent(typeof(PlayerController))]
public class Player : Actor
{
    
    public float speed;
    public float boost_speed;
    
    public Weapon main_hand;
    public Weapon off_hand;

    public List<Augment> augments;

    //Holds the functions that we call do update
    [HideInInspector]
    public UnityEvent update_boons; 


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        augments = new List<Augment>();
        actor_controller = this.GetComponent<PlayerController>();
        main_hand.holding_actor = this;

        augments.Add(this.gameObject.AddComponent<Dash>());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshBoons(){
        foreach(Augment aug in augments){
            if(aug is Boon){
                Boon boon_cast = (Boon)aug;
                boon_cast.ActivateBoon(this);
            }
        }
    }

    public bool AttemptAugmentAbility(){
        foreach(Augment aug in augments){
            if(aug is Spell){
                Spell spell_cast = (Spell)aug;
                spell_cast.OnActivate();
                return true;
            }
        }

        return false;
    }

    public bool AttemptAttack(){
        if(main_hand != null){
            main_hand.Attack();
        }

        return main_hand != null;
    }
}
