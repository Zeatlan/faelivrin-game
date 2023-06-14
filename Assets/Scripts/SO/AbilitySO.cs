using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitySO : ScriptableObject
{
    public new string name;
    public string description;
    public float damageMultiplicator;
    public int cooldown;
    [HideInInspector] public int currentCooldown;

    public virtual void Execute(CharacterInfo user, GameObject target) { }
}
