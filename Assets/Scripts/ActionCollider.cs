using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCollider : MonoBehaviour
{
    public enum ColliderType{Box, Circle};

    public ColliderType type;

    public bool debug_display;

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

    void OnDrawGizmos(){
        if(debug_display){

            System.Func<Vector3, Vector2, Vector3> apply_offset = delegate(Vector3 world_position, Vector2 offset){
                world_position.x += offset.x;
                world_position.y += offset.y;
                return world_position;
            };

            if(type == ColliderType.Box){
                if(box == null){
                    box = GetComponent<BoxCollider2D>();
                }
                Vector2 size_vector = box.size;
                Vector2 offset = box.offset;

                Vector3 world_position = this.transform.position;
                world_position = apply_offset(world_position, offset);

                if(is_player_in_action_collider){
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireCube(world_position, new Vector3(size_vector.x, size_vector.y, 1.0f));
            } else 
            if(type == ColliderType.Circle){
                if(circle == null){
                    circle = GetComponent<CircleCollider2D>();
                }

                float radius = circle.radius;
                Vector2 offset = circle.offset;

                Vector3 world_position = this.transform.position;
                world_position = apply_offset(world_position, offset);

                if(is_player_in_action_collider){
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.red;
                }

                Gizmos.DrawWireSphere(world_position, radius);
            }

        }
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
