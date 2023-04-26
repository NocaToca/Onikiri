using System.Collections;
using System.Text.RegularExpressions;
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

    NekomataAnimationController animation_controller;



    protected override void Start(){
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player");
        fire_wisps = new List<GameObject>();

        animation_controller = new NekomataAnimationController(this);
    }

    public override void PlayDamageAnimation(){
        animation_controller.GetHit();
    }
    public override void PlayCastAnimation(){
        animation_controller.Cast();
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

        animation_controller.Update();
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

public class NekomataAnimationController{

    public enum NekomataState{
        WALK,
        IDLE,
        HIT,
        CAST
    }

    public Direction current_direction; 
    public NekomataState state;
    public Animator anime;
    public Nekomata neko;

    public NekomataAnimationController(Nekomata neko){
        this.neko = neko;
        anime = neko.GetComponent<Animator>();
        if(anime == null){
            Debug.LogError("Nekomata has no animator");
        }
        state = NekomataState.IDLE;
    }

    public void Update(){

        UpdateDirection();

        if(!IsPlaying(NekomataState.HIT) && !IsPlaying(NekomataState.CAST)){
            UpdateAnimation();
        }
    }

    public void GetHit(){
        state = NekomataState.HIT;
        PlayAnimation(AssembleString());
    }
    public void Cast(){
        state = NekomataState.CAST;
        PlayAnimation(AssembleString());
    }

    public bool IsPlaying(NekomataState state){
        var check = anime.GetCurrentAnimatorClipInfo(0);
        if(check.Length != 0){
            string state_string = AssembleString(state);
            string current_clip_name = check[0].clip.name;

            if(state_string == current_clip_name){
                this.state = state;
                return true;
            } else {
                //Debug.Log("State: " + state_string);
                //Debug.Log("Clip: " + current_clip_name);
            }
        }

        return false;
    }

    private void UpdateAnimation(){
        //We can just see what state our tree is at, though we do need to add that functionality
        string state_name = neko.GetComponent<NekomataAITree>().GrabNameOfCurrentNode();

        #if UNITY_EDITOR
        //Debug.Log(state_name);
        #endif

        NekomataState new_state = state;
        if(state_name == "Walk"){
            new_state = NekomataState.WALK;
        } else 
        if(state_name == "Idle"){
            new_state = NekomataState.IDLE;
        }

        if(new_state != state){
            state = new_state;
            PlayAnimation(AssembleString());
        }

        //The other states will be called by the Neko class
    }

    private string AssembleString(){
        return AssembleString(state);
    }

    private string AssembleString(NekomataState state){
        if(state == NekomataState.WALK){
            return "Walk";
        } else
        if(state == NekomataState.HIT){
            return "Hit";
        } else
        if(state == NekomataState.CAST){
            return "Cast";
        } else {
            return "Idle";
        }
    }

    private void PlayAnimation(string name){
        //Debug.Log(name);
        anime.Play(name);
    }

    private void UpdateDirection(){
        //So, we can actually assume some things here to make our computation easier
        //The first thing is that we are always going to be walking and facing the player, so we will just assess through that

        //Altho first, we can make sure that we actually want to do things by assessing how far the player is
        Vector3 player_position = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector3 this_position = neko.transform.position;

        if(Vector3.Distance(this_position, player_position) > 50.0f){
            return;
        }

        Vector3 direction_vector = this_position - player_position;

        if(direction_vector.x > 0){
            current_direction = Direction.East;
        } else {
            current_direction = Direction.West;
        }

        if(current_direction == Direction.West){
            neko.GetComponent<SpriteRenderer>().flipX = false;
        } else {
            neko.GetComponent<SpriteRenderer>().flipX = true;
        }

    }

}