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

    public float speed;

    public float attack_speed;

    public Weapon weapon;

    public List<AIAction> available_actions; 
    
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

    public static Enemy GetEnemy(GameObject go){
        SwordEnemy sword_cast = go.GetComponent<SwordEnemy>();
        if(sword_cast != null){
            return sword_cast;
        }

        return null;
    }
}
