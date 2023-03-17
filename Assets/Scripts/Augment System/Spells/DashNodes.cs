using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Augments{

/***************************************************************************Base Dash********************************************************************************/

    [CreateAssetMenu(fileName = "Dash", menuName = "Augments/Spells/Dash/Base")]
    public class Dash : SkillNode{

        protected bool dashing;
        public float speed;
        public float distance;
        protected Vector3 destination;
        protected UnityEvent enable_event;

        public Dash(float speed, float distance, Player p) : base(p){
            this.speed = speed;
            this.distance = distance;
            this.dashing = false;
        }

        public override void ActivateEvent(){
            Debug.Log("Activated Dash upgrade base");
            active = true;
            dashing = false;
        }

        public override void UpdateEvent(){
            if(dashing){
                p.transform.position = Vector3.MoveTowards(p.transform.position, destination, speed * Game.tick);
                if(p.transform.position == destination){
                    enable_event.Invoke();
                    dashing = false;
                }
            }
        }

        //Seperated to make the upgrades easier
        public void ChooseDestination(){
            Debug.Log("Disabled");
            enable_event = new UnityEvent();
            p.ToggleMovement(true);
            enable_event.AddListener(p.ToggleMovement);
            dashing = true;

            Vector3 spawn_position = p.transform.position;
            Vector3 spawn_rotation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            spawn_rotation.z = spawn_position.z;
            float angle = 0.0f;
            if(spawn_rotation.y - spawn_position.y < 0 ){
                angle = -Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
            } else {
                angle = Mathf.Acos((spawn_rotation.x - spawn_position.x)/(spawn_position-spawn_rotation).magnitude);
            }

            Vector2 dash_direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            destination = new Vector3(dash_direction.x * distance + p.transform.position.x, dash_direction.y * distance + p.transform.position.y, 0.0f);

            p.ToggleCollider(true);
            enable_event.AddListener(p.ToggleCollider);
        }

        public override void SkillEvent(){
            ChooseDestination();

            enable_event.Invoke();
        }

        //Dash can be used with any weapon but the blade and whip
        public override bool AssessWeapon(Weapon weapon){
            return true;//!(weapon is BladeAndWhip);
        }

        public override Node Upgrade(int number){
            Debug.LogError("Dash Children are not implemented yet");
            return this;
        }
        public override Node Assemble(){
            children = new List<Node>();

            EmpherialDash c1 = new EmpherialDash(speed, distance, p);
            Blink c2 = new Blink(speed, distance, p);
            GhostDash c3 = new GhostDash(speed, distance, p);
            PhantomDash c4 = new PhantomDash(speed, distance, p);
            ArcDash c5 = new ArcDash(speed, distance, p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());
            children.Add(c3.Assemble());
            children.Add(c4.Assemble());
            children.Add(c5.Assemble());

            return this;
        }

        public override string Name(){
            return "Spirit Dash";
        }

        public override string Description(){
            string weapon_combat = "Weapons: All but the blade and whip";
            string new_line = "\n";
            string base_desc = "Dash toward your mouse direction.";

            return weapon_combat + new_line + new_line + base_desc;
        }

    }

/***************************************************************************Empherial Dash********************************************************************************/

    /*
        Regardless, makes you immune to damage. Increases distance and speed;
    
        If a melee weapon additionally deals damage on pass through
    */
    public class EmpherialDash : Dash{

        public EmpherialDash(float speed, float distance, Player p) : base(speed + 5.0f, distance + 2.0f, p){}

        public override void SkillEvent(){
            ChooseDestination();

            //We have to update the enable and disable event to also enable immunity and make it so the character damages those it passes through
            //That would be basically the main thing
            //if(p.main_hand is MeleeWeapon){
                p.ToggleOnEnterDamage(false);
                enable_event.AddListener(p.ToggleOnEnterDamage);
            //}
            
            
            enable_event.Invoke();
        }

        public override string Name(){
            return "Empherial Dash";
        }

        public override string Description(){
            string weapon_combat = "Weapons: (For All) increases range and speed. (For Melee) deals damage when passing through an enemy";
            string new_line = "\n";
            string base_desc = "Empowers dash to attack those that you pass through. Additionally increases the speed and range";
            return weapon_combat + new_line + new_line + base_desc;
        }

        public override Node Assemble(){
            ChainDash child1 = new ChainDash(speed-5.0f,distance-2.0f,p);
            AggresiveDash child2 = new AggresiveDash(speed-5.0f,distance-2.0f,p);

            children = new List<Node>();

            children.Add(child1.Assemble());
            children.Add(child2.Assemble());

            foreach(Node child in children){
                child.parent = this;
            }

            return this;
        }

    }

    //Resets cooldown and cost when dashing through a new enemy.
    public class ChainDash : EmpherialDash{
        public ChainDash(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            return this;
        }
    }

    //Applies weapon effects on hit
    public class AggresiveDash : EmpherialDash{
        public AggresiveDash(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Blink********************************************************************************/

    //Basically instead of a dash it is a teleport
    public class Blink : Dash{
        public Blink(float speed, float distance, Player p) : base(speed,distance,p){}

        //This is super simple
        public override void SkillEvent(){
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            p.transform.position = position;
        }

        public override Node Assemble(){
            children = new List<Node>();
            children.Add(new RevengeDash(speed, distance, p));
            children.Add(new QuickAttack(speed, distance, p));

            return this;
        }
    }

    //Marks targets that hit you, and you can teleport to them for free
    public class RevengeDash : Blink{
        public RevengeDash(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            return this;
        }
    }

    //Deals damage around the destination
    public class QuickAttack : Blink{
        public QuickAttack(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            return this;
        }
    }
/***************************************************************************Ghost Dash********************************************************************************/
   
    //Allows the player to dash through walls and objects
    public class GhostDash : Dash{
        public GhostDash(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            children = new List<Node>();

            NightmareDash nd = new NightmareDash(speed, distance, p);
            PhaseShift ps = new PhaseShift(speed, distance, p);
            children.Add(nd.Assemble());
            children.Add(ps.Assemble());

            return this;
        }
    }

    //Cancels cast of all enemies hit and fears them
    public class NightmareDash : GhostDash{
        public NightmareDash(float speed, float distance, Player p) : base(speed,distance,p){}

        public override Node Assemble(){
            return this;
        }
    }

    //No more dash, just makes you basically a ghost for 2 seconds that can deal damage
    public class PhaseShift : GhostDash {
        public PhaseShift(float speed, float distance, Player p) : base(speed,distance,p){}
        
        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Phantom Dash********************************************************************************/

    //Leaves behind a phantom clone that can act as a distraction
    public class PhantomDash : Dash{

        public PhantomDash(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            children = new List<Node>();

            ShadowAssault nd = new ShadowAssault(speed, distance, p);
            LivingShadow ps = new LivingShadow(speed, distance, p);
            children.Add(nd.Assemble());
            children.Add(ps.Assemble());

            return this;
        }

    }

    //Dash is now a target ability that, on target, surrounds the enemy with 4 phantoms. This can be recasted to have the phantoms deal damage in an AoE fashion, but disappear on doing so
    public class ShadowAssault : PhantomDash{

        public ShadowAssault(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            return this;
        }
    }

    //Gives the phantom AI
    public class LivingShadow : PhantomDash{

        public LivingShadow(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            return this;
        }
    }
/***************************************************************************Arc Dash********************************************************************************/
    
    //Deals damage in a AoE location around the target
    public class ArcDash : Dash{
        public ArcDash(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            children = new List<Node>();

            Fissure nd = new Fissure(speed, distance, p);
            ShortCircuit ps = new ShortCircuit(speed, distance, p);
            children.Add(nd.Assemble());
            children.Add(ps.Assemble());

            return this;
        }
    }

    //Deals damage in dash area
    public class Fissure : ArcDash{

        public Fissure(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            return this;
        }
    }

    //Increases damage, adds stun and pushback
    public class ShortCircuit : ArcDash{
        public ShortCircuit(float speed, float distance, Player p) : base(speed, distance, p){}

        public override Node Assemble(){
            return this;
        }
    }

}
