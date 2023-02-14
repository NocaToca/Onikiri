using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//We're simply going to dash forward
public class Dash : Spell
{
    UnityEvent enable_event;
    float speed = 0.5f;
    float distance = 2.0f;
    Vector3 destination;

    bool dashing;

    // Start is called before the first frame update
    void Start()
    {
        dashing = false;        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){
        if(dashing){
            this.gameObject.transform.position = Vector3.MoveTowards(this.gameObject.transform.position, destination, speed);
            if(this.gameObject.transform.position == destination){
                enable_event.Invoke();
                dashing = false;
            }
        }
    }

    //We want to disable movement and take our destination direction before dashing our character to there
    public override void OnActivate(){
        Debug.Log("Disabled");
        enable_event = this.GetComponent<Player>().DisableMovement();
        dashing = true;

        Vector3 spawn_position = this.transform.position;
        Vector3 spawn_rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawn_rotation.z = spawn_position.z;
        float angle = 0.0f;
        if(spawn_rotation.y - spawn_position.y < 0 ){
            angle = -Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
        } else {
            angle = Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
        }

        Vector2 dash_direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        destination = new Vector3(dash_direction.x * distance + this.transform.position.x, dash_direction.y * distance + this.transform.position.y, 0.0f);

        enable_event.Invoke();
    }
}
