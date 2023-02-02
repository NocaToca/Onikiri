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

    //So, using our current room we will search through every room and find a valid entrance to use, returning it
    //If you want to look through a more detailed explanation look at the comments below!
    RoomTuple ChooseValidRandomRoom(GameObject current_room, List<int> random_order){

        Entrance[] entrances = null; //Nullable since there can be a lack of valid entrance


        //We'll just go through our rooms and find what we need
        for(int i = 0; i < random_order.Count; i++){
            int index = random_order[i];

            //This will essentially look through each room randomly
            Room assessing_room = rooms[index].GetComponent<Room>();

            entrances = current_room.GetComponent<Room>().TryToFindValidEntrance(assessing_room);

            if(entrances != null){
                return new RoomTuple(rooms[index], assessing_room, entrances[0], entrances[1]);
            }
        }

        //And now we know for a fact that if we can't find an entrance there is no valid room!
        return null;
    }


    /*
    
        Since this algorithm is kind of difficult and I'm not copying off of an example (I'm looking at other algorithms though) I'm
        implementing this in stages.

        The stages go as follows:
        1. Make sure the algorithm works for two rooms, attaching them correctly and in a way that I want to (DONE)
        2. Make the algorithm iterative, constantly adding on rooms and doing so in a correct way
        3. Change the algorithm to be recursive so that we can cover all entrances 
        4. Fix potential overlap problems by making the algorithm recgonize the grid and add collision of rooms into consideration
        5. Add weights and make sure the algorithm uses weights correctly
        6. Make the algorithm find a destinition to an end room, create the end room
        7. Implement the above recursively as well, add alternation between rooms and bridges (that shouldn't be too hard)
        8. Optimize the algorithm by having it do most computation in compute buffers (fun!)

        As you can see we are currently only 1/8th of the way there. Exciting! 
    
    */

    //Using the rooms in our room list we will place them in a way that we have described
    void Generate(){

        GameObject current_room = ChooseRandomRoom();
        Vector3 chosen_position = Vector3.zero;

        /*
            Currently we are sticking strickly to not rotating rooms, but later I will have the Quaternions rotate according to orientation so
            we can not do as much work creating rooms.

            In otherwords, we will be able to create single rooms and not have to worry about making their orientation counterparts layer but for 
            now we do
        */
        Quaternion base_rotation = Quaternion.identity; 


        GameObject previous_room = Instantiate(current_room, chosen_position, base_rotation);

        //Temp variable for number of rooms
        int num_rooms = 4;


        /*
            For how ever many rooms we decide (tentative, future condition will be until we reach the exit) we want to choose a valid
            room based off of our previous room. In the future this will be done through recursion. 

            Right now we're just going to choose a random entrance from a random room we choose and then try to find a room with a fitting orient. 

            There's two main things that can happen from here:
                1. The algorithm cannot find a valid entrance (pertinent) which makes the room not valid (oh well)
                2. There are no entrances left in the room (This is good!)

            So we'd need to handle the first problem. If the room is not valid we obviously just switch to another room, but how can we determine
            if continuation of the algorithm is a lost cause? Well, we can store what rooms we have tried and check if it is as big as our room list.
            Doing this will theoritically make it stop once we've exhausted all rooms

            If there are no entrances left in the room we can stop

            We have to change how we choose a random room though, since now we don't want an entirely random number.
            What we can do instead is shuffle a list of linearly spaced numbers so our algorithm doesn't waste time generating the same number
            If this is confusing I'd recommend looking at the alternative!
        */


        for(int i = 1; i < num_rooms; i++){
            List<int> random_order = RoomHelper.CreateShuffledRandomList(rooms.Count);
            DebugHelper.DebugList<int>(random_order);

            //Our room tuple contains the game object, room, and entrance that we will be using!
            RoomTuple found_entrance = ChooseValidRandomRoom(previous_room, random_order);

            //Let's just make sure that it's not null
            if(found_entrance == null){
                //Uh-oh! No more valid rooms :c
                Debug.Log("No valid room found, exiting");
                break;
            }

            //Now we update the current room thanks to our room tuple

            //Our position instance will be for the entrance we're starting from
            Vector3 position = found_entrance.attached_entrance.transform.position;

            position -= found_entrance.entrance.transform.position;

            found_entrance.ConnectRooms();

            previous_room = Instantiate(found_entrance.room_object, position, base_rotation);
        }

    }
}
