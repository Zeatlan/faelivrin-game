using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesViewer
{
    private RangeFinder _rangeFinder;
    private List<OverlayTile> _inRangeTiles;
    private List<OverlayTile> _previewedTiles;

    public TilesViewer()
    {
        _inRangeTiles = new List<OverlayTile>();
        _previewedTiles = new List<OverlayTile>();
        _rangeFinder = new RangeFinder();
    }

    public List<OverlayTile> GetInRangeTiles() { return _inRangeTiles; }
    public List<OverlayTile> GetPreviewedTiles() { return _previewedTiles; }

    public void GetInRangeTiles(CharacterInfo character)
    {
        ResetInRangeTile();

        _inRangeTiles = _rangeFinder.GetTilesInRange(character.activeTile, character.GetStats().range);

        foreach (OverlayTile item in _inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public void GetAttackableTiles(CharacterInfo character)
    {
        ResetInRangeTile();

        _inRangeTiles = _rangeFinder.GetTilesInRange(character.activeTile, character.GetStats().atkRange, true);

        foreach (OverlayTile tile in _inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void GetPreviewAttackableTiles(OverlayTile startingTile, CharacterInfo character)
    {
        ResetPreviewedTiles();

        _previewedTiles = _rangeFinder.GetTilesInRange(startingTile, character.GetStats().atkRange, true);

        foreach (OverlayTile tile in _previewedTiles)
        {
            tile.ShowPreviewAtackableTile();
        }
    }

    public void GetSkillTiles(CharacterInfo character, AbilitySO skill)
    {
        ResetInRangeTile();

        _inRangeTiles = _rangeFinder.GetSkillRange(character.activeTile, skill, new Vector2Int(1, 0));

        foreach (OverlayTile tile in _inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void PreviewSkillLine(CharacterInfo character, MouseController mouseController)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int mouseGridPos = new Vector2Int(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y));

        RaycastHit2D? focusedTileHit = mouseController.GetFocusedOnTile();

        if (!focusedTileHit.HasValue) return;

        OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

        Vector2Int relativePos = overlayTile.grid2DLocation - character.activeTile.grid2DLocation;

        Vector2Int gridPos = new Vector2Int(Mathf.Clamp(relativePos.x, -1, 1), Mathf.Clamp(relativePos.y, -1, 1));

        ResetInRangeTile();

        AbilitySO skill = character.GetStats().skill;
        _inRangeTiles = _rangeFinder.GetSkillRange(character.activeTile, skill, gridPos);

        foreach (OverlayTile tile in _inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void PreviewDynamicSkill(CharacterInfo character, MouseController mouseController)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int mouseGridPos = new Vector2Int(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y));

        RaycastHit2D? focusedTileHit = mouseController.GetFocusedOnTile();

        if (!focusedTileHit.HasValue) return;

        OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

        Vector2Int relativePos = overlayTile.grid2DLocation - character.activeTile.grid2DLocation;

        int atkRange = character.GetStats().atkRange - 1;

        Vector2Int gridPos = new Vector2Int(Mathf.Clamp(relativePos.x, -atkRange, atkRange), Mathf.Clamp(relativePos.y, -atkRange, atkRange));

        ResetInRangeTile();

        AbilitySO skill = character.GetStats().skill;
        _inRangeTiles = _rangeFinder.GetSkillRange(character.activeTile, skill, gridPos);

        foreach (OverlayTile tile in _inRangeTiles)
        {
            tile.ShowAttackableTile();
        }

    }

    public void ResetPreviewedTiles()
    {
        List<OverlayTile> previewedTilesCopy = new List<OverlayTile>(_previewedTiles);
        foreach (OverlayTile tile in previewedTilesCopy)
        {
            tile.HidePreview();
            _previewedTiles.Remove(tile);
        }
    }

    public void ResetInRangeTile()
    {
        foreach (OverlayTile tile in _inRangeTiles)
        {
            tile.HideTile();
        }
    }
}
