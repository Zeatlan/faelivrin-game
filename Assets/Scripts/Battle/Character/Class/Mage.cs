namespace BattleSystem.Character.Class
{
    public class Mage : CharacterBase
    {
        protected override int CalculateDamage(CharacterBase unit, CharacterStats stats)
        {
            int baseDamage = stats.magicalDamage;
            int weaponDamage = 5; // TODO : Placeholder for weapon
            int totalDamage = baseDamage + weaponDamage;

            return totalDamage;
        }
    }
}