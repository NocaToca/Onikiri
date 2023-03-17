using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

    public class Companion : BoonNode{

        float damage;
        float attack_speed;

        public Companion(float damage, float attack_speed, Player p) : base(p){
            this.damage = damage;
            this.attack_speed = attack_speed;
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
                return new Companion(damage + 0.05f, attack_speed, p);
            } else {
                return new Companion(damage, attack_speed + 0.05f, p);
            }

            //return null;
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Eternal Hunger";
        }
        public override string Description(){
            return "N/A";
        }

    }

}