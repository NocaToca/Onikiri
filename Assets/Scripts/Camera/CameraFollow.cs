using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public enum FollowType{
        Player,
        Chosen
    }

    [Header("Basic Settings")]
    [Tooltip("Player: Automatically sets the follow object to the player in the scene.\nChosen: Uses the follow target you choose.")]
    public FollowType type;

    [Tooltip("The object we are following")]
    public GameObject follow;

    [Tooltip("The speed of the camera")]
    public float speed;

    void Start(){
        if(type == FollowType.Player){
            follow = GameObject.FindGameObjectWithTag("Player");
        }
    }

    //Every update, we move toward the point but lock z, which allows us to innately have a nice lerp effect
    //The reason for this is the increasing magnitude weight in z, the direction we ignore, as we get closer
    public void Update(){
        Vector3 point = Vector3.MoveTowards(this.transform.position, follow.transform.position, Game.tick * speed);
        point.z = this.transform.position.z;

        this.transform.position = point;
    }
}
