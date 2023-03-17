using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

/***************************************************************************Base Time Control********************************************************************************/

    [CreateAssetMenu(fileName = "Time Control", menuName = "Augments/Spells/Time Control/Base")]
    public class TimeControl : SkillNode
    {
        public float percentage;

        [HideInInspector]
        public bool toggled;

        public TimeControl(Player p) : base(p){
            toggled = false;

        }


        public override void ActivateEvent(){
            active = true;
            toggled = false;
        }

        public override void UpdateEvent(){

        }

        public override void SkillEvent(){
            //This is basically the idea, exceot we will obviously need to implement it better
            if(toggled){
                Game.tick = 0.01f * percentage;
            } else {
                Game.tick = 0.01f;
            }

            toggled = !toggled;
        }

        //TimeControl can be used with any weapon
        public override bool AssessWeapon(Weapon weapon){
            return true;
        }

        public override Node Assemble(){
            children = new List<Node>();

            ChronoFissure c1 = new ChronoFissure(p);
            TimeDilation c2 = new TimeDilation(p);
            Retrocausality c3 = new Retrocausality(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());
            AddChild(c3.Assemble());

            return this;
        }

        public override string Name(){
            return "Time Control";
        }

        public override string Description(){
            string weapon_combat = "Weapons: All";
            string new_line = "\n";
            string base_desc = "Slow down time for everything but you.";

            return weapon_combat + new_line + new_line + base_desc;
        }

    }


/***************************************************************************Chrono Fissure********************************************************************************/

    public class ChronoFissure : TimeControl {
        public ChronoFissure(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            TemporalPulse c1 = new TemporalPulse(p);
            TemporalBurn c2 = new TemporalBurn(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }
    }

    public class TemporalPulse : ChronoFissure {
        public TemporalPulse(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }
    
    public class TemporalBurn : ChronoFissure {
        public TemporalBurn(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Time Dilation********************************************************************************/
    public class TimeDilation : TimeControl {
        public TimeDilation(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            TemporalClock_Slow c1 = new TemporalClock_Slow(p);
            TemporalClock_Speed c2 = new TemporalClock_Speed(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }
    }

    public class TemporalClock_Slow : TimeDilation {
        public TemporalClock_Slow(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }
    
    public class TemporalClock_Speed : TimeDilation {
        public TemporalClock_Speed(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

/***************************************************************************Retrocausality********************************************************************************/
    public class Retrocausality : TimeControl {
        public Retrocausality(Player p) : base(p){}

        public override Node Assemble(){
            children = new List<Node>();

            ChronoBomb c1 = new ChronoBomb(p);
            TimeBreak c2 = new TimeBreak(p);
            AddChild(c1.Assemble());
            AddChild(c2.Assemble());

            return this;
        }
    }

    public class ChronoBomb : Retrocausality {
        public ChronoBomb(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }
    
    public class TimeBreak : Retrocausality {
        public TimeBreak(Player p) : base(p){}

        public override Node Assemble(){
            return this;
        }
    }

}

