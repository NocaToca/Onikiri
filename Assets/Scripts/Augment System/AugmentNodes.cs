using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Augments{

    public abstract class Node : ScriptableObject{

        [HideInInspector]
        public List<Node> children;

        [HideInInspector]
        public Node parent;

        [HideInInspector]
        public bool active = false;

        public Player p;

        public Image display_image;

        public Node(Player p){
            this.p = p;
        }

        //The Event that is called when the augment is activated
        public abstract void ActivateEvent();

        //Upgrades with regards to the child
        public virtual Node Upgrade(int num){
            return children[num];
        }

        //Assembles the main node structure for easy use within the tree
        public abstract Node Assemble();

        public bool IsRelated(Node other){
            if(other.Name() == this.Name()){
                return true;
            }

            Node temp = other.parent;
            while(temp != null){
                if(temp.Name() == this.Name()){
                    return true;
                }
                temp = temp.parent;
            }

            return false;
        }

        public void AddChild(Node child){
            child.parent = this;
            children.Add(child);
        }

        //Event Added to the player Update Listener
        public abstract void UpdateEvent();

        //Returns the in-game name of the node
        public abstract string Name();

        //Returns the in-game description of the node
        public abstract string Description();

        public abstract int OrderNum();

    }

    public class SkillNode : Node{

        protected SkillSettings main_settings;

        public SkillNode(Player p) : base(p){}

        public SkillNode(Player p, SkillSettings settings) : base(p){
            this.main_settings = settings;
        }
        public SkillNode(SkillNode parent) : base(parent.p){
            this.main_settings = parent.main_settings;
        }

        public virtual void SkillEvent(){

        }
        public virtual bool AssessWeapon(Weapon weapon){
            return false;
        }

        public override Node Assemble(){
            children = new List<Node>();

            Dash c1 = new Dash(15.0f, 3.0f, p);
            TimeControl c2 = new TimeControl(p);
            Rejuvenate c3 = new Rejuvenate(p);
            Vampire c4 = new Vampire(p);
            //GravityWell c5 = new GravityWell(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());
            AddChild(c3.Assemble());
            AddChild(c4.Assemble());
            //AddChild(c5.Assemble());

            return this;
        }

        

        public virtual string WeaponCompatibility(){
            return "All";
        }

        public override void UpdateEvent(){

        }

        public override string Name(){
            return "N/A";
        }
        public override string Description(){
            return "N/A";
        }

        public override void ActivateEvent(){

        }

        public override int OrderNum(){
            return -1;
        }

    }


    public class BoonNode : Node{

        protected BoonSettings main_settings;

        public BoonNode(Player p) : base(p){}

        public BoonNode(Player p, BoonSettings settings) : base(p){
            this.main_settings = settings;
        }
        public BoonNode(BoonNode parent) : base(parent.p){
            this.main_settings = parent.main_settings;
        }

        public virtual bool Updative(){
            return main_settings.type == BoonType.Updative;
        }
        public virtual void PassiveEvent(){

        }

        public override Node Assemble(){
            children = new List<Node>();

            EternalHunger c1 = new EternalHunger(0.05f,p);
            ImmortalVitality c2 = new ImmortalVitality(0.05f,p);
            DivineAegis c3 = new DivineAegis(0.05f,p);
            CelestialFury c4 = new CelestialFury(0.05f,p);
            SwiftasWind c5 = new SwiftasWind(0.05f,p);
            PurifyingLight c6 = new PurifyingLight(0.05f,p);
            HealingRain c7 = new HealingRain(0.05f,p);
            InarisBlessing c8 = new InarisBlessing(0.05f,p);
            Companion c9 = new Companion(0.05f,0.05f,p);
            MagicalAttunement c10 = new MagicalAttunement(0.05f,0.05f,p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());
            AddChild(c3.Assemble());
            AddChild(c4.Assemble());
            AddChild(c5.Assemble());
            AddChild(c6.Assemble());
            AddChild(c7.Assemble());
            AddChild(c8.Assemble());
            AddChild(c9.Assemble());
            AddChild(c10.Assemble());

            return this;
        }

        public override void UpdateEvent(){

        }

        public override string Name(){
            return "N/A";
        }
        public override string Description(){
            return "N/A";
        }

        public override void ActivateEvent(){
            
        }

        public override int OrderNum(){
            return -1;
        }

    }

    public struct SkillSettings{
        

    }
    public struct BoonSettings{
        public BoonType type;

        public BoonSettings(BoonType type){
            this.type = type;
        }
    }
}

