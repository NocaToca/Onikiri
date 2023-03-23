using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    //If the actor is stun we are not accepting movement
    public bool accepting_movement;

    public CanvasController canvas;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
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


/*
Lol I'm doing an ocaml thing in class and decided to just do it here long story short ignore this

let rec map : ('a -> 'b) -> 'a ll -> unit =
    fun f ll ->
    let rec helper no = 
    match no with
    | None -> ()
    | Some node -> node.data <- f node.data;
        helper (node.next)
    in 
    helper ll.head

*/