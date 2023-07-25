using System.Collections;
using System.Collections.Generic;
using BattleSystem.Character;
using UnityEngine;

namespace BattleSystem.SO
{
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
        public Sprite icon;
        public new string name;
        public string description;
        public float efficiencyMultiplicator;
        public int cooldown;
        public int range;
        public RangeType rangeType;
        public ZoneType zoneType;

        [HideInInspector] public int currentCooldown;

        public virtual bool Execute(CharacterBase user, OverlayTile target) { return true; }

        public virtual bool ExecuteMultipleTarget(CharacterBase user, List<OverlayTile> targets) { return true; }
    }
}