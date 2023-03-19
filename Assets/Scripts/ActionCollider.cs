using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCollider : MonoBehaviour
{
    public enum ColliderType{Box, Circle};

    CircleCollider2D circle;
    BoxCollider2D box;

    [HideInInspector]
    public bool is_player_in_action_collider;

    // Start is called before the first frame update
    void Start()
    {
        is_player_in_action_collider = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            is_player_in_action_collider = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            is_player_in_action_collider = false;
        }
    }
}
