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

    PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<PlayerController>();
        main_hand.holding_actor = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
