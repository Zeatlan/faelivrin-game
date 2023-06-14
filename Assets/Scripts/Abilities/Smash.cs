using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Smash", menuName = "Game/Battle/Abilities/Smash")]
public class Smash : AbilitySO
{
    public override void Execute(CharacterInfo user, GameObject target)
    {
        CharacterInfo targetCharacter = target.GetComponent<CharacterInfo>();

        if (targetCharacter != null)
        {
            int totalDamage = Mathf.FloorToInt(user.GetStats().attack * damageMultiplicator);
            targetCharacter.TakeDamage(totalDamage);
        }
    }
}
