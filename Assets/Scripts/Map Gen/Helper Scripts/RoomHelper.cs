using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RoomHelper{
    
    public static List<int> CreateShuffledRandomList(int length){

        List<int> r_list = new List<int>();

        for(int i = 0; i < length; i++){
            r_list.Add(i);
        }

        r_list = Shuffle<int>(r_list);

        return r_list;
    }  

    public static List<T> Shuffle<T>(List<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  

        return list;
    }

}

//Used for storing room choice
public class RoomTuple{

    public GameObject room_object;
    public Room room;
    public Entrance entrance;

    public Entrance attached_entrance;

    public RoomTuple(GameObject room_object, Room room, Entrance entrance, Entrance attached_entrance){
        this.room_object = room_object;
        this.room = room;
        this.entrance = entrance;

        this.attached_entrance = attached_entrance;
    }

    public void ConnectRooms(){
        entrance.is_connected = true;
        attached_entrance.is_connected = true;
    }

}

public enum Direction{
    North,
    South,
    East,
    West
}

//Stores the saved orientation of an entrance. Used for decided what entrance of a room to use to match 
//Room placement

//Orient may be used for more, like current facing direction, but at the moment it is going to be used for 
//the room generation
public class Orient{

    private Direction current_direction; //Primary direction
    private Direction secondary_direction; //Used for northwest, northeast, and further. Not useful for the room generation however

    //When we create an orient we *need* a starting direction
    public Orient(Direction chosen_direction){

        current_direction = chosen_direction;

    }

    public Direction GetOppositeDirection(Direction dir){
        if(dir == Direction.North){
            return Direction.South;
        }
        if(dir == Direction.South){
            return Direction.North;
        }
        if(dir == Direction.East){
            return Direction.West;
        }
        if(dir == Direction.West){
            return Direction.East;
        }

        Debug.LogWarning("Error in getting opposing direction of an orient");

        //Defaults to North
        return Direction.North;
    }

    //Gets the opposite direction of the current orientaion
    public Direction GetOppositeFacingDirection(){
        return GetOppositeDirection(current_direction);
    }

    //We define an orient to be valid (matching) if they are opposites
    public bool Validify(Orient opposing_orient){
        return GetOppositeFacingDirection() == opposing_orient.current_direction;
    }
    public bool Validify(Direction opposing_orient){
        return GetOppositeFacingDirection() == opposing_orient;
    }

}
