using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Vision : MonoBehaviour
{
    public List<Actor> actors_in_sight;
    public bool player_in_sights;

    // Start is called before the first frame update
    void Start()
    {
        player_in_sights = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            player_in_sights = true;
        }

        Actor a = Actor.ExtractActor(gameObject);
        if(a != null){
            actors_in_sight.Add(a);
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        if(other.gameObject == Game.player.gameObject){
            player_in_sights = false;
        }

        Actor a = Actor.ExtractActor(gameObject);
        if(a != null){
            if(actors_in_sight.Contains(a)){
                actors_in_sight.Remove(a);
            }
        }

        
    }
}
