using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI{

    //Main AI Tree, not meant to actually be called
    public abstract class AITree : MonoBehaviour
    {
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
            if(root != null){
                root.Evaluate();
            }
        }

        //Abstract setup
        protected abstract AINode SetUpTree();
    }

}

