using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Our main Node class for our AI

    AI Decisions work like a tree
*/
namespace AI
{
    //The current status of our AI, obvious from their names
    public enum Status{
        RUNNING,
        SUCCESS,
        FAILURE
    }

    public class AINode{

        //State
        protected Status state;

        //Parent
        public AINode parent;

        //Children
        public List<AINode> children = new List<AINode>();

        //Shared data, or the environment relative to the node
        protected Dictionary<string, object> environment;

        //Initialize environment and children
        public AINode(){
            parent = null;
            environment = new Dictionary<string, object>();
            children = new List<AINode>();
        }

        //Set the children
        public AINode(List<AINode> children){
            parent = null;
            environment = new Dictionary<string, object>();
            foreach(AINode child in children){
                AttachNode(child);
            }
        }

        //Attaches a node to their parent, making sure to set the parent correctly
        public void AttachNode(AINode node){
            node.parent = this;
            children.Add(node);
        }

        //Main Evaluate function to be implemented independently on each node instance
        public virtual Status Evaluate(){
            return Status.FAILURE;
        }

        //Sets the data in the environment
        public void SetData(string id, object value){
            environment[id] = value;
        }

        //Runs through each parent environment and tries to find the value
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

        //Clears the data from our environment if we are done with it
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

        //Finds the root of the tree from any given node
        public AINode GetRoot(){
            if(parent == null){
                return this;
            }

            AINode root = parent;
            while(root.parent != null){
                root = root.parent;
            }

            return root;
        }

        //Shorthands
        public Status FAIL(){
            state = Status.FAILURE;
            return state;
        }
        public Status SUCCESS(){
            state = Status.SUCCESS;
            return state;
        }
        public Status RUNNING(){
            state = Status.RUNNING;
            return state;
        }

    }

    /*
        Main idea of Sequence is that if we have Node A, Node B, and Node C as children, the tree will not look like this:
                    Parent
                /      |      \
            Node A   Node B   Node C

        But Like this:

        Parent
           |
        Node A
           |
        Node B
           |
        Node C

        Or, in other words, the Sequence node will run each child in sequence until one fails. This is really useful for nodes that rely on each other, like ability
        check to cast
    */
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

    /*
        Selector chooses the first available node from the children. It looks like a typical branch in a tree
    */
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
