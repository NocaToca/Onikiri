using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings{
    //The profile for debugging the player, to easily get settings back and swap between them
    [CreateAssetMenu(fileName = "Debug Profile", menuName = "Debug/Player/Debug Profile")]
    public class PlayerDebugProfile : ScriptableObject{

        [Header("Weapons")]
        public Weapon main_hand;
        public Weapon off_hand;

        [Header("Skills")]
        public Augments.SkillNode main_skill;
        public Augments.SkillNode off_skill;

        [Header("Boons")]
        public Augments.BoonNode first_boon;
        public Augments.BoonNode second_boon;
        public Augments.BoonNode third_boon;

    }

}