using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AI;

public class NecropuppetSkill : SkillComponent{
    public ActionCollider hitbox;// = new ActionCollider();
    public float damage = 5.0f;
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
        Debug.Log("Executing Necropuppet!");
    }
}

public class Necropuppet : AINode
{
    private float wait_time;

    public Necropuppet() : base(){
        wait_time = 0.0f;
    }

    public override Status Evaluate()
    {
        Enemy owner = (Enemy)GetData("pawn");
        NecropuppetSkill skill_component = owner.GetComponent<NecropuppetSkill>();
        Nekomata neko = (Nekomata)owner;

        if (skill_component == null)
        {
            Debug.LogError(owner.name + " is missing Necropuppet Component");
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
        if(!neko.puppet && !neko.has_puppet)
        {
            float skill_range = (float)GetData("Proximity Distance");

            GameObject[] all_enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject go in all_enemies){
                Actor a = Actor.ExtractActor(go);
                if(!(a is Nekomata) && a.dead){
                    if(Vector3.Distance(owner.gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) <= skill_range){
                        //We play here
                        neko.SetPuppet(go);
                        Debug.Log("Setting puppet");
                        wait_time = 0.0f;
                        neko.PlayCastAnimation();
                        return SUCCESS();
                    } 
                }   
            }

            
            
            
        }

        return FAIL();
    }
}