using System.Collections;
using System.Collections.Generic;
using BattleSystem.Character;
using BattleSystem.SO;
using UnityEngine;

namespace BattleSystem.Abilities
{
    [CreateAssetMenu(fileName = "Smash", menuName = "Game/Battle/Abilities/Smash")]
    public class Smash : AbilitySO
    {
        public override bool Execute(CharacterBase user, OverlayTile target)
        {
            CharacterBase targetCharacter = MapManager.Instance.FindCharacterOnTile(target).GetComponent<CharacterBase>();

            if (targetCharacter != null)
            {
                int totalDamage = Mathf.FloorToInt(user.stats.physicalDamage * efficiencyMultiplicator);
                targetCharacter.TakeDamage(totalDamage, DamageType.Physical);
                return true;
            }

            return false;
        }
    }
}