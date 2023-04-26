using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Spawning{

    public enum SpawnType{
        Square,
        Circle
    }

    public enum SpawnMode{
        Single,
        Continous
    }

    public class SpawnZone : MonoBehaviour
    {

        public int num_enemies_to_spawn = 1;

        public List<GameObject> enemies_to_spawn;

        public SpawnType type;
        public SpawnMode mode;

        public float radius;
        public float spawn_radius;

        public bool show_debug;

        bool active;

        // Start is called before the first frame update
        void Start()
        {
            active = false;
            //if(show_debug){
                #if UNITY_EDITOR
                StartCoroutine(StartOffset(2.0f));
                #endif
            //}
        }

        IEnumerator StartOffset(float wait_time){
            yield return new WaitForSeconds(wait_time);
            for(int i = 0; i <num_enemies_to_spawn; i++){
                Spawn();
            }

            if(mode == SpawnMode.Continous){
                while(GameObject.FindGameObjectsWithTag("Enemy").Length >= 5){
                    yield return new WaitForEndOfFrame();
                }
                StartCoroutine(StartOffset(wait_time));
            }
        }

        // Update is called once per frame
        void Update()
        {
            
            active = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, this.transform.position) <= spawn_radius;
            
        }

        public Bounds GetBounds(){
            return new Bounds(this.transform.position, radius);
        }

        private void OnDrawGizmos(){
            Gizmos.color = Color.green;
            if(show_debug){
                if(type == SpawnType.Square){   
                    Gizmos.DrawWireCube(this.transform.position, new Vector3(radius * 2.0f, radius * 2.0f, 1.0f));
                } else {
                    Gizmos.DrawWireSphere(this.transform.position, radius);
                }
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(this.transform.position, spawn_radius);
            }

            
            
        }

        public Vector3 FindRandomValueInBounds(){
            Vector3 position = new Vector3(0,0,0);
            
            if(type == SpawnType.Square){
                Bounds main_bounds = GetBounds();
                float x = Random.Range(main_bounds.top_left.x, main_bounds.top_right.x);
                float y = Random.Range(main_bounds.top_left.y, main_bounds.bottom_left.y);

                position.x = x;
                position.y = y;
            } else {
                float angle = Random.Range(0.0f, Mathf.PI * 2.0f);

                position.x = Mathf.Cos(angle);
                position.y = Mathf.Sin(angle);

                position *= radius;
            }

            return position;
        }

        //spawns a random enemy
        public void Spawn(){
            int ran = Random.Range(0, enemies_to_spawn.Count);

            GameObject enemy_object = Instantiate(enemies_to_spawn[ran]);

            Actor.ExtractActor(enemy_object).Scale();

            enemy_object.transform.position = FindRandomValueInBounds() + this.transform.position;
        }
    }

    public struct Bounds{
        public Vector3 top_left;
        public Vector3 top_right;
        public Vector3 bottom_left;
        public Vector3 bottom_right;

        public Bounds(Vector3 center, float radius){
            top_left = new Vector3(center.x - radius, center.y + radius, 0);
            top_right = new Vector3(center.x + radius, center.y + radius, 0);
            bottom_left = new Vector3(center.x - radius, center.y - radius, 0);
            bottom_right = new Vector3(center.x + radius, center.y - radius, 0);
        }

    }

}

