using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//I don't care about variables that are declared but never used
#pragma warning disable 0168


//This is a abstract class that represents a grid and keeps track of what "cells" are taken. 

//A "cell" is a 1 unit by 1 unit but not visually represented by anything. We have no limits on how big a grid can be
//What would be the best way to implement this? 

//A data structure like a hashmap would be to my knowledge, but this limits our grid size to however big we can allocate the 2d array
//Let's use it for now, though
public class GeneratorGrid : MonoBehaviour
{

    //I know this SEEMS really inefficient memory wise but these are only used for generation as well as they are bool arrays.
    //That's important bc 2 of these arrays arrays take as much memory as one single-point 1024 array (float[1024])
    //We also trash them immediately after we're done with them
    public bool[,] first_quad; //Works with positive x and y coordinates
    public bool[,] second_quad; //Works with negative x and positive y coordinates
    public bool[,] third_quad; //Works with negative x and y coordinates
    public bool[,] fourth_quad; //Works with positve x and negative y coordinates

    //Awake
    void Awake(){
        first_quad = new bool[1024,1024];
        second_quad = new bool[1024,1024];
        third_quad = new bool[1024,1024];
        fourth_quad = new bool[1024,1024];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //We have an out to avoid PBV and the compiler thinking I want to allocate new arrays
    //(I don't actually know if it will do that but I want to avoid it)
    private void ChooseArray(int x, int y, out bool[,] arr){
        if(x >= 0){
            arr = (y >= 0) ? first_quad : fourth_quad;
        } else {
            arr = (y >= 0) ? second_quad : third_quad;
        }
    }

    /*
        So we're going to have to figure out how to tell if the given room overlaps with any other room placed

        We have the center (it's a 2d vector since z does not matter), the height and the width

        So we're going to find the top left corner and then iterate through the array(s) to see if any value returns true
    
        We can use the or operator for this
    */
    public bool DoesOverlap(int height, int width, Vector3 center){

        bool return_bool = false;

        int top_left_x = (int)center.x - width/2;
        int top_left_y = (int)center.y - height/2; 

        for(int y = top_left_y; y < top_left_y + height; y++){
            for(int x = top_left_x; x < top_left_x + width; x++){

                //We might not have enough space in our arrays, so we can catch that error (but not dynamically allocate) here
                try{

                    bool[,] chosen_array;
                    ChooseArray(x,y, out chosen_array);
                    return_bool |= chosen_array[Mathf.Abs(x),Mathf.Abs(y)];

                }catch(Exception e){
                    Debug.Log("Room Placement out of bounds");

                    return_bool |= true;
                    return return_bool;
                }
            }
        }

        if(return_bool){
            Debug.Log("test");
        }

        return return_bool;
    }

    //Overrides for DoesOverlap
    public bool DoesOverlap(GameObject go){
        return DoesOverlap(go.GetComponent<Room>());
    }
    public bool DoesOverlap(Room room){
        return DoesOverlap(room.height, room.width, room.transform.position);
    }
    //For readability
    public bool DoesNotOverlap(int height, int width, Vector3 center){
        return !DoesOverlap(height, width, center);
    }

    private void DoubleAllocation(bool[,] in_arr, out bool[,] out_arr){

        //These should be the same value
        int width = in_arr.GetLength(0);
        int height = in_arr.GetLength(1);

        bool[,] copy_array = new bool[width * 2, height * 2];

        //Next we have to copy the contents before returning
        for(int x = 0; x < width; x++){
            for(int y = 0; y < height; y++){
                copy_array[x,y] = in_arr[x,y];
            }
        }

        //Then we swap the pointers
        out_arr = copy_array;
        
    }

    //Basically same format as the function above but here we're going to edit the array's value
    public void Place(int height, int width, Vector3 center){
        int top_left_x = (int)center.x - width/2;
        int top_left_y = (int)center.y - height/2; 

        for(int y = top_left_y; y < top_left_y + height; y++){
            for(int x = top_left_x; x < top_left_x + width; x++){

                bool[,] chosen_array;
                ChooseArray(x,y, out chosen_array);

                //We might not have enough space in our arrays, and since we want to place we have to allocate more
                try{

                    chosen_array[Mathf.Abs(x),Mathf.Abs(y)] = true;
                    
                }catch(Exception e){
                    Debug.Log("Room Placement out of bounds, allocating more");
                    DoubleAllocation(chosen_array, out chosen_array);
                    chosen_array[Mathf.Abs(x),Mathf.Abs(y)] = true;
                }
            }
        }
    }

    //Overrides for place
    public void Place(GameObject go){
        Place(go.GetComponent<Room>());
    }
    public void Place(Room room){
        Place(room.height, room.width, room.transform.position);
    }
}
