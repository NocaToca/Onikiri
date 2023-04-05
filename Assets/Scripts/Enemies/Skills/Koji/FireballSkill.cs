using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class FireballSkill : SkillComponent
{
    public float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Fireball : AINode{

    private float wait_time;

    public Fireball() : base(){
        wait_time = 0.0f;
    }

    //Basically, if the player is within the club action we can return success and run the club
    public override Status Evaluate()
    {
        Enemy owner = (Enemy)GetData("pawn");
        FireballSkill skill_component = owner.GetComponent<FireballSkill>();

        if (skill_component == null)
        {
            Debug.LogError(owner.name + " is missing Fire Wisp Skill Component");
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
            float skill_range = (float)GetData("Proximity Distance");

            if(Vector3.Distance(owner.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= skill_range){
                //We play here
                Koji koji = (Koji)owner;
                koji.ShootFireball();
                wait_time = 0.0f;
                return SUCCESS();
            
            } else {
                return FAIL();
            }
            //
        }
    }

}
