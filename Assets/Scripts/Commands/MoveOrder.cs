using System.Collections.Generic;
using UnityEngine;

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
        _character.Move(_path);

        if (_path.Count == 0)
        {
            _callback?.Invoke();
        }
    }

    public void Undo()
    {
        _character.SetCanMove(true);
        _character.Move(new List<OverlayTile>() { _lastTile });
    }
}