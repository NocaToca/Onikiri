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

    public override void TakeDamage(float damage){
        base.TakeDamage(damage);

        CanvasController canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
        if(!canvas.HealthBarExist(this)){
            canvas.CreateHealthBar(this);
        }
    }
}
