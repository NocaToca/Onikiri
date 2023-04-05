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

    protected override void Start(){
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        fire_wisps = new List<GameObject>();
    }

    protected void FixedUpdate(){
        foreach(GameObject wisp in fire_wisps){
            wisp.transform.position = Vector3.MoveTowards(wisp.transform.position, player.transform.position, wisp_speed * Game.tick);

            //Makeshift collider
            if(Vector3.Distance(wisp.transform.position, player.transform.position) <= 0.2f){
                player.GetComponent<Player>().TakeDamage(GetComponent<FireWispSkill>().damage);
                fire_wisps.Remove(wisp);
                Destroy(wisp);
            }
            //Debug.Log(Vector3.Distance(wisp.transform.position, player.transform.position));
        }
    }

    public void SpawnFireWisp(){
        GameObject wisp = Instantiate(fire_wisp_prefab);
        wisp.transform.position = this.transform.position;
        fire_wisps.Add(wisp);
    }

    //Have to clean up the firewisps before destroying them
    protected override void Die(){
        foreach(GameObject wisp in fire_wisps){
            Destroy(wisp);
        }
        base.Die();
    }

}
