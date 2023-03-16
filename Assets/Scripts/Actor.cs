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

    public bool immune;


    [HideInInspector]
    public Controller actor_controller;

    protected virtual void Start(){
        stats = new Stats(100.0f);
        anime = GetComponent<Animator>();
        if(anime == null){
            Debug.LogWarning("Warning: No animator found on the Game Object " + gameObject.name + ". No animations will play.");
        }

        immune = false;
    }

    void Update(){
        
    }

    

    public void PlayDamageAnimation(){
        anime.Play("Hit");
    }

    public virtual void TakeDamage(float damage){
        if(immune){
            return;
        }
        PlayDamageAnimation();
        stats.health -= damage;
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

    public Stats(float health){
        this.max_health = health;
        this.health = health;
    }

    public void AddHealth(float num){
        max_health += num;
        health += num;
    }

}