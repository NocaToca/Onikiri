using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NocaScripts
{
    //Class for any helper scripts I use

    public static List<T> FilterList<T>(System.Func<T, bool> action, List<T> objects){
        List<T> new_list = new List<T>();

        for(int i = 0; i < objects.Count; i++){
            T obj = objects[i];
            if(action(obj)){
                new_list.Add(obj);
            }
        }

        return new_list;
    }

    public static List<T> FilterList<T, G>(System.Func<T, G, bool> action, G consideration, List<T> objects){
        List<T> new_list = new List<T>();

        for(int i = 0; i < objects.Count; i++){
            T obj = objects[i];
            if(action(obj, consideration)){
                new_list.Add(obj);
            }
        }

        return new_list;
    }
}
