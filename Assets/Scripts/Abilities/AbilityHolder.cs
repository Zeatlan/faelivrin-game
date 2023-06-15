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
        ability = GetComponent<CharacterInfo>().GetStats().skill;
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
        SetOnCooldown();
    }

    public void UseSkillZone(List<OverlayTile> targets)
    {
        if (CurrentState != AbilityState.ready) return;

        Ability.ExecuteMultipleTarget(GetComponent<CharacterInfo>(), targets);
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
