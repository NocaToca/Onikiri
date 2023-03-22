using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{
    public enum WeaponPass{
        Sword,
        Bow,
        Whip,
        Orb,
        Dagger
    }

    public class SpellTree : AugmentTree
    {

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            root = new SkillNode(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
            root.Assemble();
            //root = new Rejuvenate(10.0f, 2.0f, this.GetComponent<Player>());
            if(!(root is SkillNode)){
                Debug.LogError("Incompatible node type for Spell Trees");
            }

        }

        protected override void Update(){
            if(active && Input.GetKeyDown(KeyCode.Q)){
                SkillNode skill_root = (SkillNode)root;
                skill_root.SkillEvent();
            } 
        }

        protected override void FixedUpdate(){
            if(root == null){
                return;
            }
            root.UpdateEvent();
        }

    }
}

