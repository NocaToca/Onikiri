using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCollider : MonoBehaviour
{
    public enum ColliderType{Box, Circle};

    CircleCollider2D circle;
    BoxCollider2D box;

    [HideInInspector]
    public bool player_in_action;

    // Start is called before the first frame update
    void Start()
    {
        player_in_action = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            player_in_action = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            player_in_action = false;
        }
    }
}
