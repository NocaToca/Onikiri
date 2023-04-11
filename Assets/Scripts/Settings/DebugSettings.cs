using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Use this class to set values in the editor for debug pruposes. Will autoset things to the player without you having to convolute things
namespace Settings{

    //A more readable bool
    public enum Debug{
        FALSE,
        TRUE
    }

    public class DebugSettings : MonoBehaviour
    {

        public Debug debug_active;
        public PlayerDebugProfile profile;

        // Start is called before the first frame update
        void Start()
        {
            //If we don't have a profile loaded, we don't want to do anything
            if(debug_active == Debug.FALSE || profile == null){
                Destroy(this.gameObject);
            }

            //Else we can run the rest of our start-up
            DontDestroyOnLoad(this.gameObject);
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length == 0){
                DebugHelper.LogWarning(this.gameObject, "There are no players in scene.");
            }
        }

        // Update is called once per frame
        void Update()
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if(players.Length != 0){
                SetUp(players[0]);
                Destroy(this.gameObject);
            }
        }

        private void SetUp(GameObject player){
            Player p = player.GetComponent<Player>();

            if(profile.main_hand != null){
                p.main_hand = profile.main_hand;
            }
            if(profile.off_hand != null){
                p.off_hand = profile.off_hand;
            }
            if(profile.main_skill != null){
                p.first_tree.root = profile.main_skill;
            }
            if(profile.off_skill != null){
                p.second_tree.root = profile.off_skill;
            }
            if(profile.first_boon != null){
                p.boon_trees[0].root = profile.first_boon;
            }
            if(profile.second_boon != null){
                p.boon_trees[1].root = profile.second_boon;
            }
            if(profile.third_boon != null){
                p.boon_trees[2].root = profile.third_boon;
            }

            DebugHelper.Log(this.gameObject, "Set up player!");
        }   
    }

    

}
