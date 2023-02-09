using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Helps us debug by adding on or easing functionality of the built in debugger
public static class DebugHelper{

    public static void DebugList<T>(List<T> list){

        string s = "List contents : [";

        for(int i = 0; i < list.Count; i++){
            s += list[i].ToString();
            if(i != list.Count - 1){
                s +=" , ";
            } else {
                s += "]";
            }
        }

        Debug.Log(s);

    }

    public static void DebugArray<T>(T[] list){

        string s = "List contents : [";

        for(int i = 0; i < list.Length; i++){
            s += list[i].ToString();
            if(i != list.Length - 1){
                s +=" , ";
            } else {
                s += "]";
            }
        }

        Debug.Log(s);

    }
    
}
