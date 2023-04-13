using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nekomata : Enemy
{
    //Currently this is just a place holder class bc rn most of the Logic is within the Nekomata AITree, besides spawning the wisps

    //Actually maybe the Necropuppet skill as well, but I am going to add that later
    public GameObject fire_wisp_prefab;
    
    public List<GameObject> fire_wisps;

    public float wisp_speed;

    GameObject player;

    [HideInInspector]
    public bool has_puppet;
    GameObject puppet_object;



    protected override void Start(){
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        fire_wisps = new List<GameObject>();
    }

    protected void FixedUpdate(){
        List<GameObject> alive_fire_wisps = new List<GameObject>();
        foreach(GameObject wisp in fire_wisps){
            if(wisp != null){
                wisp.transform.position = Vector3.MoveTowards(wisp.transform.position, player.transform.position, wisp_speed * Game.tick);

                //Makeshift collider
                if(Vector3.Distance(wisp.transform.position, player.transform.position) <= 0.2f){
                    player.GetComponent<Player>().TakeDamage(GetComponent<FireWispSkill>().damage);
                    Destroy(wisp);
                } else {
                    alive_fire_wisps.Add(wisp);
                }
                //Debug.Log(Vector3.Distance(wisp.transform.position, player.transform.position));
            }
        }

        fire_wisps = alive_fire_wisps;
    }

    public void SpawnFireWisp(){
        GameObject wisp = Instantiate(fire_wisp_prefab);
        wisp.transform.position = this.transform.position;
        fire_wisps.Add(wisp);
    }

    public void SetPuppet(GameObject puppet){
        this.puppet_object = puppet;
        puppet_object.GetComponent<AI.AITree>().mode = AI.AIMode.ON;
        has_puppet = true;
        Actor.ExtractActor(puppet_object).SetAsPuppet();
    }

    //Have to clean up the firewisps before destroying them
    protected override void Die(){
        foreach(GameObject wisp in fire_wisps){
            Destroy(wisp);
        }

        if(puppet_object != null){
            Destroy(puppet_object);
        }

        base.Die();
    }

}
