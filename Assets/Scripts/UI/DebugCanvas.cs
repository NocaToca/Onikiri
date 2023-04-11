using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spawning;
using UnityEngine.UI;

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
    }

}

