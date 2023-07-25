using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.SO
{
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Game/Battle/Character Stats")]
    public class CharacterStatsSO : ScriptableObject
    {
        // Informations
        [Header("Infos générale")]
        public string characterName;
        public Sprite icon;
        public AbilitySO skill;

        // Stats d'attaque
        [Header("Statistiques d'attaque")]
        public int baseHealth;
        public int baseMana;
        public int basePhysicalDamage;
        public int baseMagicalDamage;

        // Portées
        [Header("Portées")]
        public int baseRange = 3;
        public int baseAtkRange = 1;

        // Pourcentages
        [Header("Pourcentages")]
        public float baseAccuraccyRate = 0.95f;
        public float baseDodgeRate = 0.1f; // Esquive et de double coup
        public float baseCriticalRate = 0.1f;

        // Stats
        [Header("Statistiques générales")]
        public int baseStrength; // Influe sur les dégâts physique
        public int baseMagic; // Influe sur les dégâts magiques / Mana
        public int baseDexterity; // Influe sur les points de vue
        public int baseSpeed; // Influe sur la vitesse pour jouer
        public int baseLuck;
        public int baseDefense; // Résistance aux dégâts physiques
        public int baseResistance; // Résistance aux dégâts magiques
    }
}