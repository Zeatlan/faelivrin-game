namespace BattleSystem.Character.Class
{
    public class Rider : CharacterBase
    {
        protected override int CalculateDamage(CharacterBase unit, CharacterStats stats)
        {
            int baseDamage = stats.physicalDamage;
            int weaponDamage = 5; // TODO : Placeholder for weapon
            int totalDamage = baseDamage + weaponDamage;

            return totalDamage;
        }
    }
}