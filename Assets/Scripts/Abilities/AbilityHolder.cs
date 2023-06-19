using System.Collections;
using System.Collections.Generic;
using BattleSystem.SO;
using UnityEngine;
using UnityEngine.Events;

namespace BattleSystem.Abilities
{
    public class AbilityHolder : MonoBehaviour
    {
        private AbilitySO _ability;

        public enum AbilityState
        {
            ready,
            active,
            cooldown
        }
        private AbilityState _currentState = AbilityState.ready;

        public AbilityState CurrentState { get => _currentState; set => _currentState = value; }
        public AbilitySO Ability { get => _ability; set => _ability = value; }

        void Start()
        {
            _ability = GetComponent<CharacterInfo>().GetStats().skill;
        }

        private void OnEnable()
        {
            PhaseManager.OnTurnEnded.AddListener(UpdateAbilityCooldown);
        }

        private void OnDisable()
        {
            PhaseManager.OnTurnEnded.RemoveListener(UpdateAbilityCooldown);
        }

        public void UseSkill(OverlayTile target)
        {
            if (CurrentState != AbilityState.ready) return;

            bool executed = Ability.Execute(GetComponent<CharacterInfo>(), target);
            if (executed)
                SetOnCooldown();
        }

        public void UseSkillZone(List<OverlayTile> targets)
        {
            if (CurrentState != AbilityState.ready) return;

            bool executed = Ability.ExecuteMultipleTarget(GetComponent<CharacterInfo>(), targets);

            if (executed)
                SetOnCooldown();
        }

        private void SetOnCooldown()
        {
            CurrentState = AbilityState.cooldown;
            Ability.currentCooldown = Ability.cooldown;
        }

        private void UpdateAbilityCooldown()
        {
            if (CurrentState == AbilityState.ready) return;

            Ability.currentCooldown = (Ability.currentCooldown > 0) ? Ability.currentCooldown - 1 : 0;

            if (Ability.currentCooldown == 0)
            {
                CurrentState = AbilityState.ready;
            }
        }
    }
}