using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class RoomObject : ScriptableObject
{
    public GameObject prefab;
    public float weight;

    public Transform[] spawn_points;
    //public Actor[] enemy spawns

    [HideInInspector]
    public int index; //The index of the list that the RoomObject is stored at

    public Room GetRoom(){
        return prefab.GetComponent<Room>();
    }
}
