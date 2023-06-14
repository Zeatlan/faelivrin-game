using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RangeType
{
    Diamond,
    Square,
    Line,
}

public class AbilitySO : ScriptableObject
{
    public new string name;
    public string description;
    public float efficiencyMultiplicator;
    public int cooldown;
    public int range;
    public RangeType rangeType;

    [HideInInspector] public int currentCooldown;

    public virtual void Execute(CharacterInfo user, GameObject target) { }
}
