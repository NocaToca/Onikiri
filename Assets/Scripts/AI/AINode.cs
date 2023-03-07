using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public enum Status{
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class AINode{

        protected Status state;

        public AINode parent;

        public List<AINode> children;

        protected Dictionary<string, object> environment;

        public AINode(){
            parent = null;
            environment = new Dictionary<string, object>();
            children = new List<AINode>();
        }

        public AINode(List<AINode> children){
            parent = null;
            environment = new Dictionary<string, object>();
            foreach(AINode child in children){
                AttachNode(child);
            }
        }

        public void AttachNode(AINode node){
            node.parent = this;
            children.Add(node);
        }

        public virtual Status Evaluate(){
            return Status.FAILURE;
        }

        public void AddEnvironment(string id, object value){
            environment[id] = value;
        }

        public object GetData(string id){

            object return_object = null;

            if(environment.TryGetValue(id, out return_object)){
                return return_object;
            }

            AINode node = this.parent;

            while(node != null){
                return_object = node.GetData(id);
                if(return_object != null){
                    return return_object;
                }

                node = node.parent;
            }

            return null;

        }

        public bool ClearData(string id){

            if(environment.ContainsKey(id)){
                environment.Remove(id);
                return true;
            }

            AINode node = this.parent;

            while(node != null){
                bool cleared = parent.ClearData(id);

                if(cleared){
                    return true;
                }

                node = node.parent;
            }

            return false;
        }

    }

    public class Sequence : AINode{

        public Sequence() : base(){

        }
        public Sequence(List<AINode> children) : base(children){

        }

        public override Status Evaluate(){

            bool running = false;

            foreach(AINode child in children){
                switch(child.Evaluate()){
                    case Status.FAILURE:
                        state = Status.FAILURE;
                        return state;
                    case Status.SUCCESS:
                        continue;
                    case Status.RUNNING:
                        running = true;
                        continue;
                    default: 
                        state = Status.SUCCESS;
                        return state;
                }
            }

            state = (running) ? Status.RUNNING : Status.SUCCESS;

            return state;

        }

    }

    public class Selector : AINode{

        public Selector() : base(){

        }
        public Selector(List<AINode> children) : base(children){

        }

        public override Status Evaluate(){


            foreach(AINode child in children){
                switch(child.Evaluate()){
                    case Status.FAILURE:
                        continue;
                    case Status.SUCCESS:
                        state = Status.SUCCESS;
                        return state;
                    case Status.RUNNING:
                        state = Status.RUNNING;
                        return state;
                    default: 
                        continue;
                }
            }

            state = Status.FAILURE;

            return state;

        }

    }
}
