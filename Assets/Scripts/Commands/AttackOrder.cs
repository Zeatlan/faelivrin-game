public class AttackOrder : IOrder
{
    private readonly CharacterInfo _character;
    private readonly CharacterInfo _target;

    private int _damageAmount;
    private bool _isExecuted;

    public AttackOrder(CharacterInfo character, CharacterInfo target)
    {
        _character = character;
        _target = target;

        _damageAmount = _character.GetStats().attack;
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