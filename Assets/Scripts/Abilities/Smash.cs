using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Smash", menuName = "Game/Battle/Abilities/Smash")]
public class Smash : AbilitySO
{
    public override bool Execute(CharacterInfo user, OverlayTile target)
    {
        CharacterInfo targetCharacter = MapManager.Instance.FindCharacterOnTile(target);

        if (targetCharacter != null)
        {
            int totalDamage = Mathf.FloorToInt(user.GetStats().attack * efficiencyMultiplicator);
            targetCharacter.TakeDamage(totalDamage);
            return true;
        }

        return false;
    }
}
