using System.Collections.Generic;
using UnityEngine;

public class MoveOrder : IOrder
{
    private readonly CharacterInfo _character;
    private readonly List<OverlayTile> _path;

    private OverlayTile _lastTile;
    private OverlayTile _currentTargetTile;

    public MoveOrder(CharacterInfo character, List<OverlayTile> path)
    {
        _character = character;
        _path = path;
    }

    public void Execute()
    {
        _lastTile = _character.activeTile;
        _character.Move(_path);
    }

    public void Undo()
    {
        _character.SetCanMove(true);
        _character.Move(new List<OverlayTile>() { _lastTile });
    }
}