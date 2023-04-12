using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
//Will probably be abstract in the future but right now here to test weapon functionality
public class Enemy : Actor
{
    
    Rigidbody2D rb;
    BoxCollider2D cc;

    [Header("Basic Variables")]
    [Tooltip("The speed of the enemy")]
    public float speed;
    [Tooltip("How fast the enemy attacks")]
    public float attack_speed;

    [Header("Weapon")]
    [Tooltip("The weapon the enemy holders")]
    public Weapon weapon;

    [HideInInspector]//depricated
    public List<AIAction> available_actions; 

    //Is the animator or animation running for a different move?
    [HideInInspector]
    public bool playing_action;

    
    
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<BoxCollider2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Takes damage and then displays the health bar over the enemy
    public override void TakeDamage(float damage){
        base.TakeDamage(damage);
        PushBack();
    }

    //We're going to assume that we're taking damage from the player direction, but that will clearly not be the case all the time
    public void PushBack(){
        Vector3 enemy_position = this.gameObject.transform.position;
        Vector3 player_position = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 direction = enemy_position - player_position;
        direction = direction.normalized;

        direction *= incoming_force;
        Debug.Log(incoming_force);

        rb.AddForce(direction);
    }

    protected override void Die(){
        Destroy(this.gameObject);
    }
    
    public static Enemy GetEnemy(GameObject go){

        //Apparently Unity fixed the cast problem
        Enemy basic_cast = go.GetComponent<Enemy>();
        if(basic_cast != null){
            //Debug.Log("Basic Cast Success!");
            return basic_cast;
        }

        SwordEnemy sword_cast = go.GetComponent<SwordEnemy>();
        if(sword_cast != null){
            return sword_cast;
        }

        Ogre ogre_cast = go.GetComponent<Ogre>();
        if(ogre_cast != null){
            return ogre_cast;
        }

        return null;
    }
}
