using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Augments{

    namespace Display{

        public enum TypeofAugment{
            Boon,
            Skill
        }

        //Assembles the display for the augments from the UI
        public class AugmentDisplay : MonoBehaviour{

            public TypeofAugment augment_type;

            //Background image
            public Image background;

            //Name field
            public Text name_field;

            //Weapon fiel
            public Text weapon_field;

            //Display Image
            public Image display_image;

            //Description field
            public Text description_field;

            Node node;

            public void SetUpDisplay(Node node){
                name_field.text = node.Name();
                if(node is SkillNode){
                    SkillNode skill_cast = (SkillNode)node;
                    weapon_field.text = skill_cast.WeaponCompatibility();
                } else {
                    weapon_field.text = "";
                }

                display_image = node.display_image;
                description_field.text = node.Description();
                this.gameObject.SetActive(true); 

                this.node = node;
            }

            public Node SelectUpgrade(){
                return node;
            }

            public void DeactivateDisplay(){
                this.gameObject.SetActive(false);
            }

        }


    }

}


