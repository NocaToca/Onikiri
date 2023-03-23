using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCollider : MonoBehaviour
{
    public enum ColliderType{Box, Circle};
    [Header("Collider Options")]

    [Tooltip("What type of hitbox our collider has - either a box or circle")]
    public ColliderType type;

    [Header("Debug")]
    [Tooltip("Displays debug gizmos to visualize the collision")]
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

    //Draws the colliders
    void OnDrawGizmos(){
        if(debug_display){

            //A Delegate to apply an offset based off of world position
            System.Func<Vector3, Vector2, Vector3> apply_offset = delegate(Vector3 world_position, Vector2 offset){
                world_position.x += offset.x;
                world_position.y += offset.y;
                return world_position;
            };

            if(type == ColliderType.Box){
                //Box Collision Logic
                if(box == null){
                    box = GetComponent<BoxCollider2D>();
                }

                //Setting the world position and bounds
                Vector2 size_vector = box.size;
                Vector2 offset = box.offset;

                Vector3 world_position = this.transform.position;
                world_position = apply_offset(world_position, offset);

                //Red if we are not in the collider, green if we are
                if(is_player_in_action_collider){
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.red;
                }

                //Draws our wire cube!
                Gizmos.DrawWireCube(world_position, new Vector3(size_vector.x, size_vector.y, 1.0f));
            } else 
            if(type == ColliderType.Circle){
                //Circle Logic
                if(circle == null){
                    circle = GetComponent<CircleCollider2D>();
                }

                //Applies the offset and finds the world position
                float radius = circle.radius;
                Vector2 offset = circle.offset;

                Vector3 world_position = this.transform.position;
                world_position = apply_offset(world_position, offset);

                //Red if we are not in the collider, green if we are
                if(is_player_in_action_collider){
                    Gizmos.color = Color.green;
                } else {
                    Gizmos.color = Color.red;
                }

                //Draws our Wire cube!
                Gizmos.DrawWireSphere(world_position, radius);
            }

        }
    }

    //Activates and Deactivates when the player enter and leaves the collider
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
