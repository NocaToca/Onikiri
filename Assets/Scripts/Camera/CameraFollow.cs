using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public enum FollowType{
        Player,
        Chosen
    }

    public FollowType type;

    public GameObject follow;

    public float speed;

    void Start(){
        if(type == FollowType.Player){
            follow = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void Update(){
        Vector3 point = Vector3.MoveTowards(this.transform.position, follow.transform.position, Game.tick * speed);
        point.z = this.transform.position.z;

        this.transform.position = point;
    }
}
