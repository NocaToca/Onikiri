using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This is just used as a parent class for Enemies and the Player to do similar functions (namely damage functions)
public abstract class Actor : MonoBehaviour
{

    [HideInInspector]
    public bool movedLastFrame = false;
    Animator anime;

    public Stats stats;

    [HideInInspector]
    public bool immune;

    [HideInInspector]
    public UnityEvent<Weapon, Actor> damage_listener = new UnityEvent<Weapon, Actor>(); 

    [HideInInspector]
    public UnityEvent gizmos_drawn = new UnityEvent();

    [HideInInspector]
    public Controller actor_controller;

    [HideInInspector]
    public float incoming_force = 0.0f;

    [Header("Debug")]
    [Tooltip("Shows debug information related to the actor")]
    public bool debug; //enables debug action

    protected virtual void Start(){
        //SetController();

        //We create a new base Stats with a base health of 100
        stats = new Stats(100.0f);

        //Getting the animator
        anime = GetComponent<Animator>();
        if(anime == null){
            Debug.LogWarning("Warning: No animator found on the Game Object " + gameObject.name + ". No animations will play.");
        }

        immune = false;
    }

    void Update(){
        
    }


    /*
        This is used to help debug information relating to skills or anything else actor-specific, just write your specific debug and
        then add it as a listener
    */
    void OnDrawGizmos(){
        //Invokes all debug gizmos related to an actor
        gizmos_drawn.Invoke();
    }

    
    //Base hit animation
    public void PlayDamageAnimation(){
        if(anime != null){
            anime.Play("Hit");
        } else {
            Debug.Log("Hit");
        }
    }

    //We Take damage, play the damage animation, and then die if we have less than zero hp
    public virtual void TakeDamage(float damage){
        if(immune){
            damage = 0.0f;
        }
        PlayDamageAnimation();
        stats.health -= damage;
        if(stats.health <= 0){
            Die();
        }

        CanvasController canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
        if(!canvas.HealthBarExist(this)){
            canvas.CreateHealthBar(this);
        } else {
            canvas.PokeHealthBar(this);
        }
    }

    //The opposite of damage
    public void Heal(float amount){
        stats.health += amount;
        if(stats.health > stats.max_health){
            stats.health = stats.max_health;
        }
    }

    //depricated
    public void DamageOther(Weapon weapon, Actor other){
        damage_listener.Invoke(weapon, other);
    }

    //Death function
    protected virtual void Die(){

    }

    //Tries to extract an object of trpe Actor
    public static Actor ExtractActor(GameObject obj){

        Player player = obj.GetComponent<Player>();
        if(player != null){
            return player;
        }

        Enemy enemy = Enemy.GetEnemy(obj);
        if(enemy != null){
            return enemy;
        }

        return null;

    }
}

//Base Stats struct
public struct Stats{

    public float max_health {get; internal set;}
    public float percent_health {get{return health/max_health;}}
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