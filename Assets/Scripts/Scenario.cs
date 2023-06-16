using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class Scenario
    {
        private CharacterInfo _playerUnit;
        private float _distance;
        private float _health;
        public float atkPotential { get; set; }
        public float Distance { get => _distance; set => _distance = value; }
        public float Health { get => _health; set => _health = value; }
        public CharacterInfo PlayerUnit { get => _playerUnit; set => _playerUnit = value; }

        public Scenario(CharacterInfo playerUnit, float distance, float health)
        {
            PlayerUnit = playerUnit;
            Distance = distance;
            Health = health;
        }
    }
}