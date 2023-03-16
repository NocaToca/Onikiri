using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

    public class MagicalAttunement : BoonNode{

        float spell_damage;
        float enchantment_damage;

        public MagicalAttunement(float spell_damage, float enchantment_damage, Player p) : base(p){
            this.spell_damage = spell_damage;
            this.enchantment_damage = enchantment_damage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            //0 damage, 1 attack speed
            if(num == 0){
                return new MagicalAttunement(spell_damage + 0.05f, enchantment_damage, p);
            } else {
                return new MagicalAttunement(spell_damage, enchantment_damage + 0.05f, p);
            }

            //return null;
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Magical Attunement";
        }
        public override string Description(){
            return "N/A";
        }

    }

}