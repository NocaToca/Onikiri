using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Arrow : MonoBehaviour
{

    public float base_speed;

    [HideInInspector]
    public Actor sender; //Made once a arrow is instantiated 
    private Rigidbody2D rb;

    public void SetSender(GameObject sender){
        this.sender = sender.GetComponent<Actor>();
    }

    public void SetInitialVelocity(Vector2 angle){
        rb.velocity = angle * base_speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(sender == null){
            Debug.LogWarning("Arrow has no object of origin. Creation might be an error");
        }
    }

    //We want to do physics!
   
}
