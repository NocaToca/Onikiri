using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawning;
using UnityEngine.UI;
using AI;

namespace Developer{

    public class DebugCanvas : MonoBehaviour
    {
        public List<Button> buttons;
        public GameObject bg;

        public SpawnZone spawn_zone;

        // Start is called before the first frame update
        void Start()
        {
            bg.SetActive(false);
            foreach(Button b in buttons){
                b.gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Alpha1)){
                bg.SetActive(!bg.activeSelf);
                foreach(Button b in buttons){
                    b.gameObject.SetActive(!b.gameObject.activeSelf);
                }
            }
        }

        public void Spawn(){
            spawn_zone.Spawn();
        }

        bool damage_toggle = false;
        public void ToggleDamage(){
            if(damage_toggle){
                VurlnerbleAI();
            } else {
                InvincibleAI();
            }
            damage_toggle = true;
        }

        private void InvincibleAI(){
            foreach(GameObject go in spawn_zone.enemies_to_spawn){
                Actor a = Actor.ExtractActor(go);
                a.immune = true;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject go in enemies){
                Actor a = Actor.ExtractActor(go);
                a.immune = true;
            }
        }

        private void VurlnerbleAI(){
            foreach(GameObject go in spawn_zone.enemies_to_spawn){
                Actor a = Actor.ExtractActor(go);
                a.immune = false;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject go in enemies){
                Actor a = Actor.ExtractActor(go);
                a.immune = false;
            }
        }

        bool AI_toggle = false;
        public void ToggleAI(){
            if(AI_toggle){
                DisableAI();
            } else {
                EnableAI();
            }
            AI_toggle = true;
        }

        private void EnableAI(){
            foreach(GameObject go in spawn_zone.enemies_to_spawn){
                AITree ai_tree = go.GetComponent<AITree>();
                ai_tree.mode = AIMode.ON;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject go in enemies){
                AITree ai_tree = go.GetComponent<AITree>();
                ai_tree.mode = AIMode.ON;
            }
        }

        private void DisableAI(){
            foreach(GameObject go in spawn_zone.enemies_to_spawn){
                AITree ai_tree = go.GetComponent<AITree>();
                ai_tree.mode = AIMode.OFF;
            }

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject go in enemies){
                AITree ai_tree = go.GetComponent<AITree>();
                ai_tree.mode = AIMode.OFF;
            }
        }
    }

}

