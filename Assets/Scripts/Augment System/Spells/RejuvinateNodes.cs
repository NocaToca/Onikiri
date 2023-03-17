using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{


/***************************************************************************Base Rejuvenate********************************************************************************/

    [CreateAssetMenu(fileName = "Rejuvenate", menuName = "Augments/Spells/Rejuvenate/Base")]
    public class Rejuvenate : SkillNode{

        public GameObject well_prefab; //The prefab for the well
        public float life_time;

        GameObject well; //The well object

        bool well_spawned;
        float current_life;

        public Rejuvenate(Player p) : base(p){}

        public override void ActivateEvent(){
            active = true;
        }

        public override void UpdateEvent(){
            if(well_spawned){
                current_life += Game.tick;
                if(current_life > life_time){
                    GameObject.Destroy(well.gameObject);
                    current_life = 0.0f;
                    well_spawned = false;
                }
            }
        }

        public void PlayerEffect(){
            p.Heal(1.0f);
            Debug.Log("Healed");
        }

        public override void SkillEvent(){
            //Summon a well
            if(well_spawned){
                return;
            }
            well = Instantiate(well_prefab);
            well.transform.position = p.transform.position; 
            well.GetComponent<Well>().player_event.AddListener(PlayerEffect);
            well_spawned = true;
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
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());
            AddChild(c3.Assemble());

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
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

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
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

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
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

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