using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu{

    //There are many canvas controllers this one is specifically for the main menu
    public class CanvasController : MonoBehaviour
    {
        public List<Button> options;

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
            
        }

        public void ShowcaseHelp(){

        }

        public void ShowcaseOptions(){

        }

        public void LoadScene(){

        }

        public void Quit(){
            Application.Quit();
        }
    }


}

