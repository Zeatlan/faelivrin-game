using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.SO;
using BattleSystem.UI;
using UnityEngine;

namespace BattleSystem.Character
{
    public class CharacterInfo : MonoBehaviour
    {
        public OverlayTile activeTile;
        public CharacterStatsSO stats;

        private UnitPanel _unitPanel;
        private Stats _statsUI;
        public CharacterMovement characterMovement;

        public CharacterBase character;

        void Start()
        {
            InitCharacter();
            character.OnHealthChanged += UpdateHealth;
            character.OnDeath += RemoveCharacterFromList;
        }

        private void InitCharacter()
        {
            character.stats.maxHealth = stats.baseHealth;
            character.stats.currentHealth = stats.baseHealth;

            character.stats.maxMana = stats.baseMana;
            character.stats.currentMana = stats.baseMana;

            character.stats.physicalDamage = stats.basePhysicalDamage;
            character.stats.magicalDamage = stats.baseMagicalDamage;

            character.stats.range = stats.baseRange;
            character.stats.atkRange = stats.baseAtkRange;

            character.stats.accuraccyRate = stats.baseAccuraccyRate;
            character.stats.dodgeRate = stats.baseDodgeRate;
            character.stats.criticalRate = stats.baseCriticalRate;

            character.stats.strength = stats.baseStrength;
            character.stats.magic = stats.baseMagic;
            character.stats.dexterity = stats.baseDexterity;
            character.stats.speed = stats.baseSpeed;
            character.stats.luck = stats.baseLuck;
            character.stats.defense = stats.baseDefense;
            character.stats.resistance = stats.baseResistance;

            character.stats.characterName = stats.characterName;
            character.stats.icon = stats.icon;
            character.stats.skill = stats.skill;

        }

        public void DisplayInfo()
        {
            _unitPanel = GameObject.Find("UnitPanel").GetComponent<UnitPanel>();
            _unitPanel.InitializePanel(character);
        }

        private void UpdateHealth(int health)
        {
            _unitPanel?.SetHealth(health);
        }

        private void RemoveCharacterFromList()
        {
            if (MapManager.Instance.GetEnemyUnits().Contains(this))
            {
                MapManager.Instance.RemoveEnemyUnit(this);
            }
            if (MapManager.Instance.GetPlayerUnits().Contains(this))
            {
                MapManager.Instance.RemovePlayerUnit(this);
            }
        }
    }
}