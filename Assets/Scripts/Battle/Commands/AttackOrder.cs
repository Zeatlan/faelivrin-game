
using BattleSystem.Character;

namespace BattleSystem.Commands
{
    public class AttackOrder : IOrder
    {
        private readonly CharacterBase _character;
        private readonly CharacterBase _target;

        private int _damageAmount;
        private bool _isExecuted;

        public AttackOrder(CharacterBase character, CharacterBase target)
        {
            _character = character;
            _target = target;

            _damageAmount = _character.stats.physicalDamage;
            _isExecuted = false;
        }

        public void Execute()
        {
            if (!_isExecuted)
            {
                _character.Attack(_target);
                _isExecuted = true;
            }
        }

        public void Undo()
        {
            _target.HealHealth(_damageAmount);
        }

        public bool IsComplete()
        {
            return _isExecuted;
        }
    }
}