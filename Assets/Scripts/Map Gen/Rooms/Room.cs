using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int height = 5;
    public int width = 5;

    public List<Entrance> entrances;
    public int num_entrances {get {return entrances.Count;}}


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetRandomEntrancePosition(){
        return entrances[Random.Range(0, num_entrances)].transform.position;
    }
}
