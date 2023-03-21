using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The enemy controller is just going to do basic logic for weighing each action and then performing them
public class EnemyController : Controller
{

    public Enemy attached_enemy;

    public Vision sight;

    List<AIAction> possible_actions; //excludes movement

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        attached_enemy = Enemy.GetEnemy(this.gameObject);
        if(attached_enemy == null){
            Debug.LogWarning("Enemy Controller has no enemy attached, or the enemy is not set up to be found through the enemy class");
        }
    }

    // Update is called once per frame
    //Fixed update because this also handles movement
    void FixedUpdate()
    {
        FindAction();
    }

    void FindAction(){

        if(possible_actions.Count == 0){
            Debug.LogWarning("No actions have been assigned to the enemy " + gameObject.name + ".");
            return;
        }

        AIAction highest_weighted = possible_actions[0];
        float highest_weight = -10.0f; //There's no way for a weight to be negative anyway
        foreach(AIAction action in possible_actions){
            float weight = action.assessing_pointer();
            weight *= action.weight;
            if(weight > highest_weight){
                highest_weight = weight;
                highest_weighted = action;
            }
        }

        if(highest_weight == 0.0f){
            Move();
            return;
        } 

        highest_weighted.action();

    }

    void Move(){
        Vector3 dir = Vector3.MoveTowards(this.gameObject.transform.position, Game.player.gameObject.transform.position, attached_enemy.speed * Game.normalizing_speed_constant);
        this.gameObject.transform.position = dir;
    }
}

public class AIAction{

    //Pointer the the function that determines the base weight
    internal System.Func<float> assessing_pointer;

    //Action do actually use the move
    internal System.Action action;

    //Additional weight for the moves
    internal float weight;

    public AIAction(System.Func<float> action_pointer, System.Action action, float weight){
        this.assessing_pointer = action_pointer;
        this.weight = weight;
        this.action = action;
    }

}
