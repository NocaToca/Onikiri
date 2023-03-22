using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Augments{

    namespace Display{

        public enum DisplayType{
            Skill,
            Boon
        }

        //Just a helper class that acts as an intermediate between upgrading the augments and canvas interaction
        public class CanvasCommunicator{

            public AugmentDisplay[] boons;
            public AugmentDisplay[] skills;

            public CanvasCommunicator(AugmentDisplay[] boons, AugmentDisplay[] skills){
                if(skills.Length != 3 || boons.Length != 5){
                    Debug.LogError("Canvas Communicator should have an array of length 5 and 3.");
                }

                this.boons = boons;
                this.skills = skills;
            }

            //This is the chunk of logic to display each upgrade
            public void ChooseDisplayUpgrades(Player p, DisplayType type){
                List<AugmentTree> trees = p.GetAugmentTrees();

                Node[] chosen_nodes = null;

                if(type == DisplayType.Skill){
                    chosen_nodes = new Node[3];

                    List<SpellTree> skill_trees = new List<SpellTree>();
                    foreach(AugmentTree tree in trees){
                        if(tree is SpellTree){
                            SpellTree cast = (SpellTree)tree;
                            skill_trees.Add(cast);
                        }
                    }


                    List<Node> all_available_skills = new List<Node>();

                    foreach(Node n in skill_trees[0].GetChildren()){
                        all_available_skills.Add(n);
                    }
                    foreach(Node n in skill_trees[1].GetChildren()){
                        all_available_skills.Add(n);
                    }

                    List<Node> possible_nodes = new List<Node>();

                    //Now we just have to filter out what nodes we already have
                    for(int i = 0; i < all_available_skills.Count; i++){
                        for(int x = 0; x < skill_trees.Count; x++){
                            if(!skill_trees[x].root.IsRelated(all_available_skills[i])){
                                possible_nodes.Add(all_available_skills[i]);
                            }
                        }
                    }

                    for(int i = 0; i < chosen_nodes.Length; i++){
                        int ran = Random.Range(0, all_available_skills.Count);
                        chosen_nodes[i] = all_available_skills[ran];

                        all_available_skills.RemoveAt(ran);
                    }
                } else {
                    chosen_nodes = new Node[5];

                    List<BoonTree> boon_tree = new List<BoonTree>();
                    foreach(AugmentTree tree in trees){
                        if(tree is BoonTree){
                            BoonTree cast = (BoonTree)tree;
                            boon_tree.Add(cast);
                        }
                    }

                    if(boon_tree[0] == null){
                        Debug.Log(boon_tree.Count);
                        Debug.Log(trees.Count);
                        Debug.LogError("Error in Displaying Boon Trees: Tree was found to be null");
                    }

                    Debug.Log(boon_tree[0].gameObject.name);


                    List<Node> all_available_skills = new List<Node>();

                    foreach(BoonTree single_boon_tree in boon_tree){
                        List<Node> children = single_boon_tree.GetChildren();

                        foreach(Node n in children){
                            all_available_skills.Add(n);
                        }
                    }

                    List<Node> possible_nodes = new List<Node>();

                    //Now we just have to filter out what nodes we already have
                    for(int i = 0; i < all_available_skills.Count; i++){
                        for(int x = 0; x < boon_tree.Count; x++){
                            if(!boon_tree[x].root.IsRelated(all_available_skills[i])){
                                possible_nodes.Add(all_available_skills[i]);
                            }
                        }
                    }

                    for(int i = 0; i < chosen_nodes.Length; i++){
                        int ran = Random.Range(0, all_available_skills.Count);
                        chosen_nodes[i] = all_available_skills[ran];

                        all_available_skills.RemoveAt(ran);
                    }
                }

                for(int i = 0; i < chosen_nodes.Length; i++){
                    if(type == DisplayType.Skill){
                        skills[i].SetUpDisplay(chosen_nodes[i]);
                    } else {
                        boons[i].SetUpDisplay(chosen_nodes[i]);
                    }
                }

            }

        }

    }

}