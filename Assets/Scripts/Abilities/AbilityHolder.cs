using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AbilityHolder : MonoBehaviour
{
    private AbilitySO ability;

    public enum AbilityState
    {
        ready,
        active,
        cooldown
    }
    private AbilityState _currentState = AbilityState.ready;

    public AbilityState CurrentState { get => _currentState; set => _currentState = value; }
    public AbilitySO Ability { get => ability; set => ability = value; }

    void Start()
    {
        Ability = GetComponent<CharacterInfo>().stats.skill;
    }

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

        Ability.Execute(GetComponent<CharacterInfo>(), target);
        CurrentState = AbilityState.cooldown;
    }

    private void UpdateAbilityCooldown()
    {
        if (CurrentState == AbilityState.ready) return;

        Ability.currentCooldown = (Ability.currentCooldown > 0) ? Ability.currentCooldown-- : 0;

        if (Ability.currentCooldown == 0)
        {
            CurrentState = AbilityState.ready;
        }
    }
}
