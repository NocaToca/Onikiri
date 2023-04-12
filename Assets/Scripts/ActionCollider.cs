using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionCollider : MonoBehaviour
{
    public enum ColliderType{Box, Circle, None};
    public enum DetectType{Player, Enemy}
    [Header("Collider Options")]

    [Tooltip("What type of hitbox our collider has - either a box or circle")]
    public ColliderType type;
    public DetectType scan_type;

    [Header("Debug")]
    [Tooltip("Displays debug gizmos to visualize the collision")]
    public bool debug_display;

    CircleCollider2D circle;
    BoxCollider2D box;

    [HideInInspector]
    public bool is_player_in_action_collider;

    System.Action<Actor> function;

    [HideInInspector]
    public float force = 10.0f;

    
    List<GameObject> enemies = new List<GameObject>();

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

    public void ApplyToEnemies(System.Action<Actor> function){
        this.function = function;
        foreach(GameObject go in enemies){
            if(go != null){
                Actor enemy = Actor.ExtractActor(go);
                enemy.incoming_force = force;
                function(enemy);
            }
        }   
    }

    //Activates and Deactivates when the player enter and leaves the collider
    private void OnTriggerEnter2D(Collider2D other){
        if(scan_type == DetectType.Player){
            if(other.gameObject == Game.player.gameObject){
                is_player_in_action_collider = true;
            }
        } else {
            if(other.gameObject.tag == "Enemy"){
                //Debug.Log("Beep");
                if(!enemies.Contains(other.gameObject)){
                //Debug.Log("Boop");
                    if(Game.player.attacking){
                        if(function != null){
                            Actor enemy = Actor.ExtractActor(other.gameObject);
                            enemy.incoming_force = force;
                            function(Actor.ExtractActor(other.gameObject));
                        }
                    }
                    enemies.Add(other.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other){
        if(scan_type == DetectType.Player){
            if(other.gameObject == Game.player.gameObject){
                is_player_in_action_collider = false;
            }
        } else {
            if(other.gameObject.tag == "Enemy"){
                List<GameObject> flushed_objects = new List<GameObject>();
                foreach(GameObject go in enemies){
                    if(go != null && go != other.gameObject){
                        flushed_objects.Add(go);
                    }
                }
                enemies = flushed_objects;
            }
        }
    }
}
