using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RangeType
{
    Diamond,
    Square,
    Line,
}

public enum ZoneType
{
    SingleTarget,
    ZoneTarget
}

public class AbilitySO : ScriptableObject
{
    public new string name;
    public string description;
    public float efficiencyMultiplicator;
    public int cooldown;
    public int range;
    public RangeType rangeType;
    public ZoneType zoneType;

    [HideInInspector] public int currentCooldown;

    public virtual void Execute(CharacterInfo user, GameObject target) { }

    public virtual void ExecuteMultipleTarget(CharacterInfo user, List<OverlayTile> targets) { }
}
