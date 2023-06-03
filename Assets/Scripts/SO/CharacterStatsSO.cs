using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Game/Battle/Character Stats")]
public class CharacterStatsSO : ScriptableObject
{
    public string characterName;
    public int baseHealth;
    public int baseAttack;
    public int baseRange;
    public Sprite icon;
}
