using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace MainMenu{

    //There are many canvas controllers this one is specifically for the main menu
    public class CanvasController : MonoBehaviour
    {
        public List<Button> options;

        public GameObject options_menu;
        public GameObject help_menu;

        public int scene_number = 2;

        // Start is called before the first frame update
        void Start()
        {
            DebugCheck();
        }

        void DebugCheck(){
            if(options.Count != 4){
                if(options.Count > 4){
                    DebugHelper.LogWarning(this.gameObject, "Option list of buttons should be equal to four. There are currently more.");
                } else {
                    DebugHelper.LogWarning(this.gameObject, "Option list of buttons should be equal to four. There are currently less.");
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)){
                ExitCurrentMenu();
            }
        }

        public void ExitCurrentMenu(){
            foreach(Button button in options){
                button.gameObject.SetActive(true);
            }

            help_menu.SetActive(false);
            options_menu.SetActive(false);
        }

        public void ShowcaseHelp(){
            foreach(Button button in options){
                button.gameObject.SetActive(false);
            }

            help_menu.SetActive(true);
        }

        public void ShowcaseOptions(){
            foreach(Button button in options){
                button.gameObject.SetActive(false);
            }

            options_menu.SetActive(true);
        }

        public void LoadScene(){
            SceneManager.LoadScene(2);
        }

        public void Quit(){
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

            Application.Quit();
        }
    }


}

