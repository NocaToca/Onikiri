using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Keep in mind that third tier skill upgrades do not need to be implemented yet, as we are only doing the first area and you can't get those till the third
//Second tier don't even have to be implemented

//For boons first and second tier have to be implemented but third tier does not
namespace Augments{

    public class AugmentTree : MonoBehaviour
    {

        public Node root;
        public bool active = true;

        // Start is called before the first frame update
        protected virtual void Start()
        {
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            
        }

        protected virtual void FixedUpdate(){
            root.UpdateEvent();
        }

        public List<Node> GetChildren(){
            return root.children;
        }
    }

}

