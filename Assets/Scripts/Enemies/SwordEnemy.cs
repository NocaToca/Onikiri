using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : Enemy
{
    //The sword enemy is going to have a slash attack, slam attack, and a dash attack
    public ActionCollider slam_trigger;
    public ActionCollider dash_trigger;
    public ActionCollider slash_trigger;

    public float slam_weight = 1.0f;
    public float dash_weight = 1.0f;
    public float slash_weight = 1.0f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        AIAction slam_action = new AIAction(AssessingSlam, Slam, slam_weight);
        AIAction dash_action = new AIAction(AssessingDash, Dash, dash_weight);
        AIAction slash_action = new AIAction(AssessingSlash, Slash, slash_weight);

        available_actions.Add(slam_action);
        available_actions.Add(dash_action);
        available_actions.Add(slash_action);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Slash(){
        Debug.Log("Slashing");
    }
    public void Slam(){
        Debug.Log("Slamming");
    }
    public void Dash(){
        Debug.Log("Dashing");
    }

    //Basically finds the player and sees if they are in front of them
    public float AssessingSlash(){

        //First, we get the player
        Player player = Game.player;

        if(slam_trigger.player_in_action){
            //Player is in our action. Later this will be more complicated math relating to player health, movement, and cooldown but for now we just return this
            return 1.0f;
        } else {
            //Player is not in here, so we don't even want to consider this action
            return 0.0f;
        }
    }

    public float AssessingDash(){
        //First, we get the player
        Player player = Game.player;

        if(slam_trigger.player_in_action){
            //Player is in our action. Later this will be more complicated math relating to player health, movement, and cooldown but for now we just return this
            return 1.0f;
        } else {
            //Player is not in here, so we don't even want to consider this action
            return 0.0f;
        }
    }

    public float AssessingSlam(){
        //First, we get the player
        Player player = Game.player;

        if(slam_trigger.player_in_action){
            //Player is in our action. Later this will be more complicated math relating to player health, movement, and cooldown but for now we just return this
            return 1.0f;
        } else {
            //Player is not in here, so we don't even want to consider this action
            return 0.0f;
        }
    }
}
