using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class ClubSlamSkill : SkillComponent
{
    public ActionCollider hitbox = new ActionCollider();
    public float damage = -1.0f;
    public float cooldown = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Execute()
    {
        Debug.Log("Executing Club Slam!");
    }
}

public class ClubSlamNode : AINode
{
    private float wait_time;

    public ClubSlamNode() : base()
    {
        wait_time = 0.0f;
    }

    //Basically, if the player is within the club action we can return success and run the club
    public override Status Evaluate()
    {
        Enemy owner = (Enemy)GetData("pawn");
        ClubSlamSkill skill_component = owner.GetComponent<ClubSlamSkill>();

        if (skill_component == null)
        {
            Debug.LogError(owner.name + " is missing Club Slam Skill Component");
            return FAIL();
        }

        if (owner.playing_action)
        {
            return FAIL();
        }

        if (wait_time < skill_component.cooldown)
        {
            wait_time += Game.tick;
            return FAIL();
        }
        else
        {
            if (skill_component.hitbox == null)
            {
                Debug.Log("Test Debug: Club Slam is off CD, but there is no hitbox, so it will not execute.");
                return FAIL();
            }

            if (skill_component.hitbox.is_player_in_action_collider)
            {
                wait_time = 0.0f;
                skill_component.Execute();
                return SUCCESS();
            }
        }
        return FAIL();
    }
}
