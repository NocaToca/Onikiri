using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

//Walks in random directions
public class WalkNode : AINode
{
    public float speed;
    private Actor actor;

    //Wait time in between walking between each point
    private float wait_time;

    //radius to choose new destination
    private float radius;
    private float radius_buffer;

    private bool waiting;

    private float time_spent_waiting;

    private Vector3 destination;

    //Sets up the walk node with random settings
    private void Init(Actor actor, WalkSettings settings){
        this.actor = actor;
        speed = settings.speed;
        wait_time = settings.wait_time;
        radius = settings.radius;
        waiting = true;
        radius_buffer = settings.radius_buffer;

        time_spent_waiting = 0.0f;

        AINode root = GetRoot();
        
        root.SetData("Speed", speed);
    }

    public WalkNode(WalkSettings settings, Actor actor) : base(){
        Init(actor, settings);
    }
    public WalkNode(WalkSettings settings, Actor actor, List<AINode> children) : base(children){
        Init(actor, settings);
    }

    //Chooses a new random destination to walk to based off our radius
    private Vector3 ChooseNewDestination(){
        float angle = Random.Range(0.0f, 90.0f);

        float x, y;

        int x_sign = Random.Range(0,2);
        x = (x_sign % 2 == 0) ? -1 * Mathf.Cos(angle) : Mathf.Cos(angle);

        int y_sign = Random.Range(0,2);
        y = (y_sign % 2 == 0) ? -1 * Mathf.Sin(angle) : Mathf.Sin(angle);

        x *= radius;
        y *= radius;

        x += actor.transform.position.x;
        y += actor.transform.position.y;

        return new Vector3(x,y,0.0f);
    }

    public override Status Evaluate(){

        //If we are waiting, we idle until we have waited long enough and choose a new destination
        if(waiting){
            time_spent_waiting += 1.0f * Game.tick; 
            if(time_spent_waiting >= wait_time){
                time_spent_waiting = 0.0f;
                waiting = false;
                destination = ChooseNewDestination();
            }
        } else {

            //If we aren't, we just move towards the destination
            if(Vector3.Distance(actor.transform.position, destination) <= 0.01f){
                waiting = true;
            } else {
                actor.transform.position = Vector3.MoveTowards(actor.transform.position, destination, speed * Game.tick);
            }
        }

        state = Status.RUNNING;
        return state;
    }

}

public struct WalkSettings{
    public float speed;
    public float wait_time;
    public float radius;

    public float radius_buffer;

    public WalkSettings(float speed, float wait_time, float radius, float radius_buffer){
        this.speed = speed;
        this.wait_time = wait_time;
        this.radius = radius;
        this.radius_buffer = radius_buffer;
    }

}
