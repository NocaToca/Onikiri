using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Augments{

/**************************************************************Lifesteal******************************************************************/
    public class EternalHunger : BoonNode{

        float percentage;

        public EternalHunger(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new EternalHunger(percentage + 0.05f, p);
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

/**************************************************************Max Health Increase******************************************************************/
    public class ImmortalVitality : BoonNode{

        float percentage;

        public ImmortalVitality(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            float value = percentage;

            if(percentage == 0.1f){
                value = 0.25f;
            } else 
            if(percentage == 0.25f){
                value = .5f;
            }

            return new ImmortalVitality(percentage, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Immortal Vitality";
        }
        public override string Description(){
            return "N/A";
        }

    }

/**************************************************************Resistance Increase******************************************************************/
    public class DivineAegis : BoonNode{

        float percentage;

        public DivineAegis(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new DivineAegis(percentage + 0.05f, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Divine Aegis";
        }
        public override string Description(){
            return "N/A";
        }

    }

/**************************************************************Damage Increase******************************************************************/
    public class CelestialFury : BoonNode{

        float percentage;

        public CelestialFury(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            float value = percentage;

            if(percentage == 0.1f){
                value = 0.15f;
            } else 
            if(percentage == 0.15f){
                value = .25f;
            }

            return new CelestialFury(percentage, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Celestial Fury";
        }
        public override string Description(){
            return "N/A";
        }

    }
/**************************************************************Attack Speed Increase******************************************************************/
    //Swift as the Wind
    public class SwiftasWind : BoonNode{

        float percentage;

        public SwiftasWind(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new SwiftasWind(percentage + 0.05f, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Swift as the Wind";
        }
        public override string Description(){
            return "N/A";
        }

    }
/**************************************************************Negative Status Duration******************************************************************/
    //Purifying Light
    public class PurifyingLight : BoonNode{

        float percentage;

        public PurifyingLight(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new PurifyingLight(percentage + 0.05f, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Purifying Light";
        }
        public override string Description(){
            return "N/A";
        }

    }
/**************************************************************Passive Healing (Per Second)******************************************************************/
    //Healing Rain
    public class HealingRain : BoonNode{

        float percentage;

        public HealingRain(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new HealingRain(percentage + 0.05f, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Healing Rain";
        }
        public override string Description(){
            return "N/A";
        }

    }
/**************************************************************Kitsunebi Regeneration******************************************************************/
    //Inari's Blessing
    public class InarisBlessing : BoonNode{

        float percentage;

        public InarisBlessing(float percentage, Player p) : base(p){
            this.percentage = percentage;
        }

        public override void ActivateEvent(){

        }

        public override void UpdateEvent(){

        }

        public override void PassiveEvent(){

        }

        public override Node Upgrade(int num){
            return new InarisBlessing(percentage + 0.05f, p);
        }

        public override Node Assemble(){
            return this;
        }

        public override string Name(){
            return "Inari's Blessing";
        }
        public override string Description(){
            return "N/A";
        }

    }


}