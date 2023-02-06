using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    //Contains all of our rooms that we will use to generate
    public List<RoomObject> rooms;

    public RoomObject start_room; //weight does not matter
    public RoomObject end_room; //weight does not matter

    //We use this so we wont frustrate ourselves
    public bool EditingWeights;

    GeneratorGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        if(EditingWeights){
            Debug.LogWarning("EditingWeights bool still turned on in the Room Generator script. This can cause issues if the sum of the weights is not one!");
        }

        grid = this.gameObject.AddComponent<GeneratorGrid>();
        GenerateRecursive();
        
        //We're going to want to remove the component once we've generated the room so we don't keep the memory allocation
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Chooses a random room from the list using the weights (currently all are weighted equal)
    GameObject ChooseRandomRoom(){
        return rooms[Random.Range(0, rooms.Count)].prefab;
    }

    //So, using our current room we will search through every room and find a valid entrance to use, returning it
    //If you want to look through a more detailed explanation look at the comments below!
    RoomTuple ChooseValidRandomRoom(GameObject current_room, List<int> random_order){

        Entrance[] entrances = null; //Nullable since there can be a lack of valid entrance


        //We'll just go through our rooms and find what we need
        for(int i = 0; i < random_order.Count; i++){
            int index = random_order[i];

            //This will essentially look through each room randomly
            Room assessing_room = rooms[index].prefab.GetComponent<Room>();

            entrances = current_room.GetComponent<Room>().TryToFindValidEntrance(assessing_room);

            if(entrances != null){
                return new RoomTuple(rooms[index].prefab, assessing_room, entrances[0], entrances[1]);
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
        2. Make the algorithm iterative, constantly adding on rooms and doing so in a correct way (DONE)
        3. Change the algorithm to be recursive so that we can cover all entrances (DONE)
        4. Fix potential overlap problems by making the algorithm recgonize the grid and add collision of rooms into consideration (DONE - though not tested well)
        5. Add weights and make sure the algorithm uses weights correctly (DONE - although it may not work entirely as intended rn)
        6. Make the algorithm find a destinition to an end room, create the end room
        7. Implement the above recursively as well, add alternation between rooms and bridges (that shouldn't be too hard)
        8. Optimize the algorithm by having it do most computation in compute buffers (fun! Will be left out till later however bc it's not that important)

        As you can see we are currently only 4/8th of the way there. Exciting! 
        P.S I'm keeping all of the old code I make as a way of not only looking back but showcasing my process for this in case anyone wants to comment on it or improve it - or tell me I'm really stupid
        Shouldn't matter bc I trust C#'s compiler to ignore the functions I do not use
    
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

    private int[] CreateIntWeights(){
        int[] return_weights = new int[rooms.Count];
        for(int i = 0; i < rooms.Count; i++){
            return_weights[i]  = (int)(rooms[i].weight * 1000.0f);
        }
        return return_weights;
    }

    //I'm not deleting the above so I can go for fallback as well as reference
    //This shall be our recursive version of the generation
    //Recursion can be very dangerous and expensive! Hopefully we don't have much problems with it :prayge:
    //Ascii should support my fox emotes fr
    private void GenerateRecursive(){
        //We're going to have a starting point here. It doesn't matter for now but later it will be important
        //To do somethings before going right into recursion

        DebugResetAttachtment();

        //Although we can do the first step here
        GameObject current_room = ChooseRandomRoom();
        Vector3 chosen_position = Vector3.zero;
        Quaternion base_rotation = Quaternion.identity; 
        GameObject previous_room = Instantiate(current_room, chosen_position, base_rotation);

        //Now let's just place our room on the Generator Grid
        grid.Place(previous_room);

        //This is so we don't constantantly place rooms together that can recurse with themselves
        int max_room_depth = 5;

        //Let's initialize our weights
        int[] current_weights;

        //An important thing to keep in mind is that our int array is 1/1000. We store it in an integer array to help memory problems as well as make the algorithms faster
        //We don't need super precise room weights as it generally does not make much sense. But anyway, we need to convert it to our int list
        current_weights = CreateIntWeights();

        int[] copy_weights = new int[rooms.Count];

        //We don't want the weights from a previous recurse to affect the weights of the current one
        System.Array.Copy(current_weights, copy_weights, rooms.Count);
        RecurseGenerationStep(previous_room, 1, max_room_depth, copy_weights);


    }

    //Refactors our weights to have a sum of 1000 (or a little under)
    private void AdjustWeights(int[] in_weights, out int[] out_weights){
        float[] to_float = new float[in_weights.Length];

        float sum = 0.0f;
        for(int i = 0; i < in_weights.Length; i++){
            to_float[i] = in_weights[i]/1000.0f;
            sum += to_float[i];
        }

        //Now go through and adjust
        for(int i = 0; i < in_weights.Length; i++){
            to_float[i] /= sum;
            in_weights[i] = (int)(to_float[i] * 1000.0f);
        }

        //return
        out_weights = in_weights;

        //uhh idk why I'm using a bunch of out functions I'm just used to it from GLSL ig lmfao
    }

    //Now we just use the idea of the algorithm below
    private int GetRandomIndexFromWeights(int[] weights){
        int ran = Random.Range(0, weights.Length);

        int current_sum = 0;
        for(int i = 0; i < weights.Length; i++){
            current_sum += weights[i];
            if(ran < current_sum){
                return i;
            }
        }

        return weights.Length - 1;
    }

    private void RecurseGenerationStep(GameObject current_room, int current_depth, int max_room_depth, int[] current_weights){

        //If we are at the edge we want to stop!
        if(current_depth >= max_room_depth){
            return;
        }
        Quaternion base_rotation = Quaternion.identity; 

        //Else let's do the generation
        //Basically we will want to generate a room for each entrance
        //Let's do this by iterating through each entrance

        //We do have a function to do what we are going to do in a really similar way in the room class, but the difference there
        //Is that we will not have access to recursion, so we have to do it again here
        Room current_room_room_component = current_room.GetComponent<Room>();

        foreach(Entrance current_entrance in current_room_room_component.entrances){
            //Let's create a random list 
            //List<int> r_list = RoomHelper.CreateShuffledRandomList(rooms.Count); //dead code, doesn't work with weights

            /*
                So, why did I actually use an integer list instead of keeping the floats?
                Well, I want to think about this integer list as a list of lengths of lists.

                In otherwords, if we have [500, 500] we have two lists of 500, one filled with the index of 0 and the other 1
                Similarly, for [333,333,334] we have two of 333, and one of 334 with indexes 0,1,2

                So why does this help us? Well, we can generate a random number 0-999 since we know the sum is 1000. Then we will similuate int_list[rand] (or choosing the index through the concatnated list)
                by just choosing where the index falls within the lengths

                We can also deal with already searched rooms by "removing" the list of indices from the concatnated list. However, since we want to think of this smartly, we know we will
                be inputting the weights into the recursed call. 

                We're going to have to create a copy!
            */

            int[] effective_weights = new int[current_weights.Length];
            System.Array.Copy(current_weights,effective_weights,current_weights.Length);

            //Now we shall grab a random room and assess it much like the ChooseValidRandomRoom() function
            //Now it's weighted though!
            for(int i = 0; i < rooms.Count; i++){
                //int index = r_list[i];
                int index = GetRandomIndexFromWeights(effective_weights);

                GameObject assessing_room_object = rooms[index].prefab;
                Room assessing_room = assessing_room_object.GetComponent<Room>();
                for(int k = 0; k < assessing_room.entrances.Length; k++){
                    Entrance potential_attaching_entrance = assessing_room.entrances[k];
                    if(current_entrance.is_connected == false &&
                        
                        current_entrance.IsValidForOrientation(potential_attaching_entrance.main_direction)){
                        //Attach
                        Vector3 position = current_entrance.transform.position;
                        position -= potential_attaching_entrance.transform.position;

                        if(grid.DoesNotOverlap(assessing_room.height, assessing_room.width, position)){
                            GameObject created_object = Instantiate(assessing_room_object, position, base_rotation);
                            grid.Place(created_object);

                            current_entrance.is_connected = true;
                            created_object.GetComponent<Room>().entrances[k].is_connected = true;

                            int[] copy_weights = new int[rooms.Count];
                            System.Array.Copy(current_weights, copy_weights, rooms.Count);

                            //Making sure the room is reset to a weight of zero
                            copy_weights[index] = 0;
                            AdjustWeights(copy_weights, out copy_weights);

                            RecurseGenerationStep(created_object, current_depth + 1, max_room_depth, copy_weights);
                            i = rooms.Count; //To break the second for loop
                            break;
                        }

                        
                    }
                }
            }
        }

        
    }


    //All the base objects that are stored inside the room list should not have their entrances connected
    //This is a debug function to reset them in case they have for some reason
    private void DebugResetAttachtment(){
        for(int i = 0; i < rooms.Count; i++){
            foreach(Entrance entrance in rooms[i].prefab.GetComponent<Room>().entrances){
                entrance.is_connected = false;
            }
        }
    }

    //We divide our weights by the sum
    private void FixUpWeights(float sum){
        for(int i = 0; i < rooms.Count; i++){
            rooms[i].weight /= sum;
        }
    }

    /*
        On each validate we want to make sure the sum of the percentages equal one and if they do not weight them to equal one unless an
        editing bool is turned on
    */
    private void OnValidate() {
        if(!EditingWeights){
            float sum = 0.0f;
            for(int i = 0; i < rooms.Count; i++){
                sum += rooms[i].weight; 
            }

            if(sum != 1.0f){
                Debug.Log("Sum of weights does not add up to one, fixing weights.");
                FixUpWeights(sum);
            }
        }
    }

}

