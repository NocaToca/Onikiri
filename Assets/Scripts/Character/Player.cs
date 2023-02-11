using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        actor_controller = this.GetComponent<PlayerController>();
        main_hand.holding_actor = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool AttemptAttack(){
        if(main_hand != null){
            main_hand.Attack();
        }

        return main_hand != null;
    }
}
