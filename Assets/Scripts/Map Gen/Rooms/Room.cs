using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int height = 5;
    public int width = 5;

    public Entrance[] entrances;
    public int num_entrances {get {return entrances.Length;}}


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

    public Entrance[] TryToFindValidEntrance(Room attaching_room){

        Entrance[] r_array = new Entrance[2];
        //Apparently for each loops are wildly inefficient for list - which is why we use an array!

        //Basically we're just going to iterate through each entrance and see if we can attach the room to it
        //Should be at most O(16) since the max entrannces a room should have is 4
        foreach(Entrance entrance in entrances){

            //We just wanna make sure that our entrance isn't connected already (don't wanna stack rooms on top of each other!)
            if(!entrance.is_connected){
                foreach(Entrance attaching_entrance in attaching_room.entrances){
                    if(!(attaching_entrance.is_connected) && entrance.IsValidForOrientation(attaching_entrance.main_direction)){
                        r_array[0] = attaching_entrance;
                        r_array[1] = entrance;
                        return r_array; //We will return the valid entrance
                    }
                }
            }
            
        }

        return null;

    }
}
