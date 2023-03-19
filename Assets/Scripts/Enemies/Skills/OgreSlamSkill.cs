using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class OgreSlamSkill : SkillComponent
{
    public ActionCollider hitbox;
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
        Debug.Log("Executing Ogre Slam!");
    }
}

public class OgreSlamNode : AINode
{
    private float wait_time;

    public OgreSlamNode() : base()
    {
        wait_time = 0.0f;
    }

    //Basically, if the player is within the roar action we can return success and run the club
    public override Status Evaluate()
    {
        Enemy owner = (Enemy)GetData("pawn");
        OgreSlamSkill skill_component = owner.GetComponent<OgreSlamSkill>();

        if (skill_component == null)
        {
            Debug.LogError(owner.name + " is missing Ogre Slam Component");
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
                Debug.Log("Test Debug: Ogre Slam is off CD, but there is no hitbox, so it will not execute.");
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
