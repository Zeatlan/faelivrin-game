using System.Collections.Generic;
using UnityEngine;
using CharacterInfo = BattleSystem.Character.CharacterInfo;

namespace BattleSystem.Commands
{
    public delegate void MoveOrderCallback();

    public class MoveOrder : IOrder
    {
        private readonly CharacterInfo _character;
        private readonly List<OverlayTile> _path;
        private MoveOrderCallback _callback;

        private OverlayTile _lastTile;
        private OverlayTile _currentTargetTile;

        public MoveOrder(CharacterInfo character, List<OverlayTile> path, MoveOrderCallback callback = null)
        {
            _character = character;
            _path = path;
            _callback = callback;
        }

        public void Execute()
        {
            _lastTile = _character.activeTile;
            _character.characterMovement.Move(_path, _character);

            if (_path.Count == 0)
            {
                _callback?.Invoke();
            }
        }

        public void Undo()
        {
            _character.character.CanMove = true;
            _character.characterMovement.Move(new List<OverlayTile>() { _lastTile }, _character);
        }
    }
}