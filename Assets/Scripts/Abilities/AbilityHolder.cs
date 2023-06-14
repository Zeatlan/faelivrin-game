using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityHolder : MonoBehaviour
{
    public AbilitySO ability;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    private AbilityState _currentState = AbilityState.ready;

    public AbilityState CurrentState { get => _currentState; set => _currentState = value; }

    private void OnEnable()
    {
        PhaseManager.OnTurnEnded.AddListener(UpdateAbilityCooldown);
    }

    private void OnDisable()
    {
        PhaseManager.OnTurnEnded.RemoveListener(UpdateAbilityCooldown);
    }

    public void UseSkill(GameObject target)
    {
        if (CurrentState != AbilityState.ready) return;

        ability.Execute(GetComponent<CharacterInfo>(), target);
        CurrentState = AbilityState.cooldown;
    }

    private void UpdateAbilityCooldown()
    {
        if (CurrentState == AbilityState.ready) return;

        ability.currentCooldown = (ability.currentCooldown > 0) ? ability.currentCooldown-- : 0;

        if (ability.currentCooldown == 0)
        {
            CurrentState = AbilityState.ready;
        }
    }
}
