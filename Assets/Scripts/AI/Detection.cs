using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AI;

public class EnemyDetection : AINode{

    private Actor a;
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

        if(a.debug == true){
            a.gizmos_drawn.AddListener(Visualize);
        }

        if(target == null){
            Vector2 point = new Vector2(a.transform.position.x, a.transform.position.y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(point, fov);

            // Debug.Log(colliders.Length);

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

        if(Vector3.Distance(player.transform.position, a.transform.position) > 0.01f){
            a.transform.position = Vector3.MoveTowards(a.transform.position, player.transform.position, speed * Game.tick);
        }

        state = Status.RUNNING;
        return state;
    }

}
