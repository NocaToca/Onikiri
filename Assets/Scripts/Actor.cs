using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is just used as a parent class for Enemies and the Player to do similar functions (namely damage functions)
public abstract class Actor : MonoBehaviour
{
    public bool movedLastFrame = false;

    void Update(){
        
    }
    
    //Tries to extract an object of trpe Actor
    public static Actor ExtractActor(GameObject obj){

        Player player = obj.GetComponent<Player>();
        if(player != null){
            return player;
        }

        Enemy enemy = obj.GetComponent<Enemy>();
        if(enemy != null){
            return enemy;
        }

        return null;


    }
}
