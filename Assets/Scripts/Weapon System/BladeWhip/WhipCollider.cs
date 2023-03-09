using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhipCollider : MonoBehaviour
{
    Collider2D collider;

    public List<GameObject> enemies = new List<GameObject>();

    public bool enemies_in_collider {get{return enemies.Count != 0;}}

    private void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Enemy"){
            enemies.Add(col.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.tag == "Enemy"){
            enemies.Remove(col.gameObject);
        }
    }
}
