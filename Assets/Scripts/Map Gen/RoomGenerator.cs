using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    //Contains all of our rooms that we will use to generate
    public List<GameObject> rooms;

    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Chooses a random room from the list using the weights (currently all are weighted equal)
    GameObject ChooseRandomRoom(){
        return rooms[Random.Range(0, rooms.Count)];
    }

    //Using the rooms in our room list we will place them in a way that we have described
    void Generate(){

        GameObject current_room = ChooseRandomRoom();
        Vector3 chosen_position = Vector3.zero;
        Quaternion base_rotation = Quaternion.identity; 


        GameObject previous_room = Instantiate(current_room, chosen_position, base_rotation);

        int num_rooms = 2;

        for(int i = 1; i < num_rooms; i++){
            current_room = ChooseRandomRoom();

            Vector3 position = previous_room.GetComponent<Room>().GetRandomEntrancePosition();

            position -= current_room.GetComponent<Room>().GetRandomEntrancePosition();

            previous_room = Instantiate(current_room, position, base_rotation);
        }

    }
}
