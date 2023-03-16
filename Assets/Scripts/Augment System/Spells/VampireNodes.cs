using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

/***************************************************************************Base Vampire********************************************************************************/

    //Basically all the logic for bleed stacks 
    public class Vampire : SkillNode
    {
        public Vampire(Player p) : base(p){}

        public override void UpdateEvent(){

        }
        public override void ActivateEvent(){
            active = true;
        }
        public override void SkillEvent(){

        }

        public override bool AssessWeapon(Weapon weapon){
            return !(weapon is RangedWeapon);
        }

        public override Node Assemble(){
            children = new List<Node>();

            Hemophage c1 = new Hemophage(p);
            Lacerate c2 = new Lacerate(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

            return this;
        }


        public override string Name(){
            return "Vampire";
        }

        public override string Description(){
            string weapon_combat = "Weapons: No Range";
            string new_line = "\n";
            string base_desc = "Applies bleed stacks to enemies. Pop bleed stacks by casting to heal health.";

            return weapon_combat + new_line + new_line + base_desc;
        }
    }

/***************************************************************************Hemophage********************************************************************************/

    public class Hemophage : Vampire{
        public Hemophage(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Ichor c1 = new Ichor(p);
            Sanguine c2 = new Sanguine(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

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
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

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


