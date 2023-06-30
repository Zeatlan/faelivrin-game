using System.Collections;
using System.Collections.Generic;
using BattleSystem.Character;
using BattleSystem.SO;
using UnityEngine;
using CharacterInfo = BattleSystem.Character.CharacterInfo;

namespace BattleSystem.Abilities
{
    [CreateAssetMenu(fileName = "Arrows rain", menuName = "Game/Battle/Abilities/ArrowsRain")]
    public class ArrowsRain : AbilitySO
    {
        public override bool ExecuteMultipleTarget(CharacterBase user, List<OverlayTile> targets)
        {
            List<CharacterBase> targetedCharacters = new List<CharacterBase>();

            // True = Player Unit | False = Ennemy Unit
            bool isUserAPlayerUnit = MapManager.Instance.GetPlayerUnits().Contains(user.GetComponent<CharacterInfo>());

            foreach (OverlayTile tile in targets)
            {
                CharacterInfo searchCharacter = MapManager.Instance.FindCharacterOnTile(tile);

                if (searchCharacter != null)
                {
                    bool isTargetAPlayerUnit = MapManager.Instance.GetPlayerUnits().Contains(searchCharacter);

                    if (isUserAPlayerUnit != isTargetAPlayerUnit)
                    {
                        targetedCharacters.Add(searchCharacter.GetComponent<CharacterBase>());
                    }
                }
            }

            if (targetedCharacters.Count == 0) return false;

            int totalDamage = Mathf.FloorToInt(user.stats.physicalDamage * efficiencyMultiplicator);

            foreach (CharacterBase unit in targetedCharacters)
            {
                unit.TakeDamage(totalDamage, DamageType.Physical);
            }

            return true;
        }
    }
}
