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

    void Start(){
        if(type == FollowType.Player){
            follow = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void Update(){
        Vector3 point = follow.transform.position;
        point.z = this.transform.position.z;

        this.transform.position = point;
    }
}
