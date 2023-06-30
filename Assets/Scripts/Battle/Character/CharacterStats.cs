
using BattleSystem.SO;
using UnityEngine;

namespace BattleSystem.Character
{
    public class CharacterStats
    {
        CharacterClass characterClass;

        public int maxHealth;
        public int currentHealth;

        public int maxMana;
        public int currentMana;

        public int physicalDamage;
        public int magicalDamage;

        public int range;
        public int atkRange;

        public float accuraccyRate;
        public float dodgeRate;
        public float criticalRate;

        public int strength;
        public int magic;
        public int dexterity;
        public int speed;
        public int luck;
        public int defense;
        public int resistance;

        public string characterName;
        public Sprite icon;
        public AbilitySO skill;
    }
}