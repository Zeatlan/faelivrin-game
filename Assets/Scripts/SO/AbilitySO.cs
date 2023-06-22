using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        public virtual bool Execute(CharacterInfo user, OverlayTile target) { return true; }

        public virtual bool ExecuteMultipleTarget(CharacterInfo user, List<OverlayTile> targets) { return true; }
    }
}