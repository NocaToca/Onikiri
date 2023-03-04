using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //If the actor is stun we are not accepting movement
    public bool accepting_movement;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        accepting_movement = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Controller GetController(GameObject go){
        PlayerController pc = go.GetComponent<PlayerController>();
        if(pc != null){
            return pc;
        }

        EnemyController ec = go.GetComponent<EnemyController>();
        if(ec != null){
            return ec;
        }
        return null;
    }
}
