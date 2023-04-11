using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI{

    public enum AIMode{
        OFF,
        ATTACK_ONLY,
        MOVE_ONLY,
        ON
    }

    //Main AI Tree, not meant to actually be called
    public abstract class AITree : MonoBehaviour
    {
        [Header("AI")]
        [Tooltip("Determines what parts of the AI will play")]
        public AIMode mode;

        //The main root of the tree, or the start action
        protected AINode root;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            //Setups our tree to our desired specifications
            root = SetUpTree();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            //Evaluate our node in a fixed interval
            if(mode != AIMode.OFF && root != null){
                root.Evaluate();
            }
        }

        //Abstract setup
        protected abstract AINode SetUpTree();
    }

}

