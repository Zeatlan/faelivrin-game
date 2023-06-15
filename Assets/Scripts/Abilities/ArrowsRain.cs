using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Arrows rain", menuName = "Game/Battle/Abilities/ArrowsRain")]
public class ArrowsRain : AbilitySO
{
    public override bool ExecuteMultipleTarget(CharacterInfo user, List<OverlayTile> targets)
    {
        List<CharacterInfo> targetedCharacters = new List<CharacterInfo>();

        foreach (OverlayTile tile in targets)
        {
            CharacterInfo searchCharacter = MapManager.Instance.FindCharacterOnTile(tile);

            if (searchCharacter != null)
            {
                targetedCharacters.Add(searchCharacter);
            }
        }

        if (targetedCharacters.Count == 0) return false;

        int totalDamage = Mathf.FloorToInt(user.GetStats().attack * efficiencyMultiplicator);

        foreach (CharacterInfo unit in targetedCharacters)
        {
            unit.TakeDamage(totalDamage);
        }

        return true;
    }
}
