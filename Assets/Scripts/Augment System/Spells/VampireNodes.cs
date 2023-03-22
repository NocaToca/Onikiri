using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

/***************************************************************************Base Vampire********************************************************************************/

    [CreateAssetMenu(fileName = "Vampire", menuName = "Augments/Spells/Vampire/Base")]
    //Basically all the logic for bleed stacks 
    public class Vampire : SkillNode
    {
        public Vampire(Player p) : base(p){}

        public float bleed_damage_per_second;
        public float bleed_pop_damage;
        public float heal_percentage;

        Dictionary<Actor, int> inflicted_enemies;

        public override void UpdateEvent(){
            foreach(KeyValuePair<Actor, int> pair in inflicted_enemies){
                float dmg = pair.Value * bleed_damage_per_second;
                pair.Key.TakeDamage(dmg);
            }

        }

        public void AddBleed(Weapon weapon, Actor enemy){
            int num = 0;

            if(inflicted_enemies.ContainsKey(enemy)){
                inflicted_enemies[enemy] += 1;
            } else {
                num = 1;
                inflicted_enemies.Add(enemy, num);
            }
        }

        public override void ActivateEvent(){
            inflicted_enemies = new Dictionary<Actor, int>();
            p.damage_listener.AddListener(AddBleed);
            active = true;
        }

        public override void SkillEvent(){
            float heal = 0.0f;
            foreach(KeyValuePair<Actor, int> pair in inflicted_enemies){
                float dmg = pair.Value * bleed_pop_damage;
                pair.Key.TakeDamage(dmg);
                heal += dmg;
            }
            inflicted_enemies = new Dictionary<Actor, int>();

            p.Heal(heal * heal_percentage);
        }

        public override bool AssessWeapon(Weapon weapon){
            return !(weapon is RangedWeapon);
        }

        public override Node Assemble(){
            children = new List<Node>();

            Hemophage c1 = new Hemophage(p);
            Lacerate c2 = new Lacerate(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }


        public override string Name(){
            return "Vampire";
        }
#pragma warning disable 0219

        public override string Description(){
            string weapon_combat = "Weapons: No Range";
            string new_line = "\n";
            string base_desc = "Applies bleed stacks to enemies. Pop bleed stacks by casting to heal health.";

            return base_desc;
        #pragma warning restore 0219
        }
    }

/***************************************************************************Hemophage********************************************************************************/

    public class Hemophage : Vampire{
        public Hemophage(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Ichor c1 = new Ichor(p);
            Sanguine c2 = new Sanguine(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }

    }

    public class Ichor : Hemophage {
        public Ichor(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

    public class Sanguine : Hemophage {
        public Sanguine(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Lacerate********************************************************************************/

    public class Lacerate : Vampire{
        public Lacerate(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Maul c1 = new Maul(p);
            Infection c2 = new Infection(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }

    }

    public class Maul : Lacerate {
        public Maul(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

    public class Infection : Lacerate {
        public Infection(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }
}


