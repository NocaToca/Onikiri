using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI{

    public abstract class AITree : MonoBehaviour
    {
        private AINode root;

        // Start is called before the first frame update
        protected virtual void Start()
        {
            root = SetUpTree();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if(root != null){
                root.Evaluate();
            }
        }

        protected abstract AINode SetUpTree();
    }

}

