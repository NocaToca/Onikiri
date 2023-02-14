using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Enchantments just give our weapons more flair
public class Enchantment 
{
    //We have a value for each stat, but they don't have to be above zero
    public float[] weapon_stat_increase;

    //Handles DoT damage, special weapon damage calculations... that sorta thing
    public System.Func<float> Weapon_Effect;

    public Enchantment(){
        
    }

    public static Enchantment GenerateRandomEnchantment(){
        return new Enchantment();
    }

}
