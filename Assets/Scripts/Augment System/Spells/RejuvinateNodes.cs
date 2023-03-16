using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{


/***************************************************************************Base Rejuvenate********************************************************************************/

    public class Rejuvenate : SkillNode{

        public Rejuvenate(Player p) : base(p){}

        public override void ActivateEvent(){
            active = true;
        }

        public override void UpdateEvent(){

        }

        public override void SkillEvent(){
            //Summon a well
        }

        //
        public override bool AssessWeapon(Weapon weapon){
            return true;
        }

        public override Node Assemble(){
            children = new List<Node>();

            Revitalize c1 = new Revitalize(p);
            Empower c2 = new Empower(p);
            Devour c3 = new Devour(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());
            children.Add(c3.Assemble());

            return this;
        }

        public override string Name(){
            return "Rejuvinate";
        }

        public override string Description(){
            string weapon_combat = "Weapons: Orb and Dagger";
            string new_line = "\n";
            string base_desc = "Creates a well that passively heals you while inside.";

            return weapon_combat + new_line + new_line + base_desc;
        }

    }

/***************************************************************************Revitalize********************************************************************************/

    public class Revitalize : Rejuvenate{
        public Revitalize(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Engross c1 = new Engross(p);
            Absorb c2 = new Absorb(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

            return this;
        }

    }

    public class Engross : Revitalize {
        public Engross(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

    public class Absorb : Revitalize {
        public Absorb(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Empower********************************************************************************/

    public class Empower : Rejuvenate{
        public Empower(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Embolden c1 = new Embolden(p);
            Invigor c2 = new Invigor(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

            return this;
        }

    }

    public class Embolden : Empower {
        public Embolden(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

    public class Invigor : Empower {
        public Invigor(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Devour********************************************************************************/

    public class Devour : Rejuvenate{
        public Devour(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            Gorge c1 = new Gorge(p);
            Ravage c2 = new Ravage(p);
            children.Add(c1.Assemble());
            children.Add(c2.Assemble());

            return this;
        }

    }

    public class Gorge : Devour {
        public Gorge(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

    public class Ravage : Devour {
        public Ravage(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

}