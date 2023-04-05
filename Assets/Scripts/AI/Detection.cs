using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

//Our main detection node
public class EnemyDetection : AINode{

    private Actor a;

    //Our field of view, or radius for now
    public float fov;

    public EnemyDetection(Actor a, float fov) : base(){
        this.a = a;
        this.fov = fov;
    }
    public EnemyDetection(Actor a, float fov, List<AINode> children) : base(children){
        this.a = a;
        this.fov = fov;
    }

    public override Status Evaluate(){
        object target = GetData("target");

        //If our debug variable is true for our actor, we draw the view distance
        if(a.debug == true){
            a.gizmos_drawn.AddListener(Visualize);
        }

        //If we don't have a target, we try to find one
        if(target == null){

            //3d to 2d
            Vector2 point = new Vector2(a.transform.position.x, a.transform.position.y);

            //Grabbing all of the colliders in the fov
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, fov);

            // Debug.Log(colliders.Length);

            //Look through each collission and see if it is the player, if it is great! we will add it to the root environment
            GameObject player = null;
            foreach(Collider2D col in colliders){
                if(col.gameObject.tag == "Player"){
                    player = col.gameObject;
                    break;
                }
            }

            if(player == null){
                state = Status.FAILURE;
                return state;
            }

            AINode root = GetRoot();

            root.SetData("target", player);
            Debug.Log("Target acquired: " + player.name);
        } else
        {
            // Debug.Log("Already has Target");
        }

        state = Status.SUCCESS;
        return state;
    }

    //Gizmo logic
    public void Visualize(){
        object target = GetData("target");

        //We don't see
        if(target == null){
            Gizmos.color = Color.red;
        } else {
            //We see
            GameObject player = (GameObject)target;
            if(Vector3.Distance(a.transform.position, player.transform.position) <= fov){
                //We in range
                Gizmos.color = Color.blue;
            } else {
                Gizmos.color = Color.red;
            }   
        }

        Gizmos.DrawWireSphere(a.transform.position, fov);
    }
}

//Actually moves our character to the target
public class GoToTarget :AINode{
    private Actor a;

    float speed;

    public GoToTarget(Actor a, float speed) : base(){
        this.a = a;
        this.speed =speed;
    }

    public GoToTarget(Actor a, float speed, List<AINode> children) : base(children){
        this.a = a;
        this.speed= speed;
    }

    public override Status Evaluate(){
        GameObject player = (GameObject)GetData("target");

        object data = GetData("Proximity Distance");

        float walk_to_distance = 0.01f;

        if(data != null){
            walk_to_distance = (float)data;
        }

        //If our target is not close enough we move to it and look towards it
        if(Vector3.Distance(player.transform.position, a.transform.position) > walk_to_distance){
            a.transform.position = Vector3.MoveTowards(a.transform.position, player.transform.position, speed * Game.tick);
            a.transform.up= Vector3.Lerp(a.transform.up, (player.transform.position - a.transform.position), 0.1f);
        }

        state = Status.RUNNING;
        return state;
    }

}
