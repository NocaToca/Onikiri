using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This is just used as a parent class for Enemies and the Player to do similar functions (namely damage functions)
public abstract class Actor : MonoBehaviour
{
    public bool movedLastFrame = false;
    Animator anime;

    public Stats stats;


    [HideInInspector]
    public Controller actor_controller;

    protected virtual void Start(){
        stats = new Stats(100.0f);
        anime = GetComponent<Animator>();
        if(anime == null){
            Debug.LogWarning("Warning: No animator found on the Game Object " + gameObject.name + ". No animations will play.");
        }
    }

    void Update(){
        
    }

    //Returns a unity event that is bound to the re-enable function. 
    public UnityEvent DisableMovement(){
        actor_controller.accepting_movement = false;
        UnityEvent return_event = new UnityEvent();
        return_event.AddListener(EnableMovement);
        return return_event;
    }
    
    protected virtual void Die(){

    }

    public void EnableMovement(){
        actor_controller.accepting_movement = true;
        Debug.Log("Enabled");
    } 

    public void PlayDamageAnimation(){
        anime.Play("Hit");
    }

    public virtual void TakeDamage(float damage){
        PlayDamageAnimation();
        stats.health -= damage;
        if(stats.health <= 0.0f){
            Die();
        }
    }

    //Tries to extract an object of trpe Actor
    public static Actor ExtractActor(GameObject obj){

        Player player = obj.GetComponent<Player>();
        if(player != null){
            return player;
        }

        Enemy enemy = obj.GetComponent<Enemy>();
        if(enemy != null){
            return enemy;
        }

        return null;

    }
}

public struct Stats{

    public float max_health {get; internal set;}
    public float health {get; internal set;}
    public float percent_health {get{return health/max_health;}}

    public Stats(float health){
        this.max_health = health;
        this.health = health;
    }

    public void AddHealth(float num){
        max_health += num;
        health += num;
    }

}