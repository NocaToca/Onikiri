using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{
    public enum BoonType{
        Singular,
        Updative
    }

    public class BoonTree : AugmentTree
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            root = new BoonNode(GameObject.FindGameObjectWithTag("Player").GetComponent<Player>());
            if(!(root is BoonNode)){
                Debug.LogError("Incompatible node type for BoonTree");
            }
        }

    }
}

