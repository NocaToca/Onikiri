using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //If the actor is stun we are not accepting movement
    public bool accepting_movement;

    public CanvasController canvas;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
