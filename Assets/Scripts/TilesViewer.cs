using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesViewer
{
    private RangeFinder rangeFinder;
    private List<OverlayTile> inRangeTiles;
    private List<OverlayTile> previewedTiles;

    public TilesViewer()
    {
        inRangeTiles = new List<OverlayTile>();
        previewedTiles = new List<OverlayTile>();
        rangeFinder = new RangeFinder();
    }

    public List<OverlayTile> GetInRangeTiles() { return inRangeTiles; }
    public List<OverlayTile> GetPreviewedTiles() { return previewedTiles; }

    public void GetInRangeTiles(CharacterInfo character)
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.GetStats().range);

        foreach (OverlayTile item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public void GetAttackableTiles(CharacterInfo character)
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.GetStats().atkRange, true);

        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void GetPreviewAttackableTiles(OverlayTile startingTile, CharacterInfo character)
    {
        ResetPreviewedTiles();

        previewedTiles = rangeFinder.GetTilesInRange(startingTile, character.GetStats().atkRange, true);

        foreach (OverlayTile tile in previewedTiles)
        {
            tile.ShowPreviewAtackableTile();
        }
    }

    public void GetSkillTiles(CharacterInfo character, AbilitySO skill)
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetSkillRange(character.activeTile, skill, new Vector2Int(1, 0));

        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void PreviewSkillLine(CharacterInfo character, AbilitySO skill, MouseController mouseController)
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2Int mouseGridPos = new Vector2Int(Mathf.RoundToInt(mouseWorldPos.x), Mathf.RoundToInt(mouseWorldPos.y));

        RaycastHit2D? focusedTileHit = mouseController.GetFocusedOnTile();

        if (!focusedTileHit.HasValue) return;

        OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();

        Vector2Int relativePos = overlayTile.grid2DLocation - character.activeTile.grid2DLocation;

        Vector2Int gridPos = new Vector2Int(Mathf.Clamp(relativePos.x, -1, 1), Mathf.Clamp(relativePos.y, -1, 1));

        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetSkillRange(character.activeTile, skill, gridPos);

        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.ShowAttackableTile();
        }

    }

    public void ResetPreviewedTiles()
    {
        List<OverlayTile> previewedTilesCopy = new List<OverlayTile>(previewedTiles);
        foreach (OverlayTile tile in previewedTilesCopy)
        {
            tile.HidePreview();
            previewedTiles.Remove(tile);
        }
    }

    public void ResetInRangeTile()
    {
        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.HideTile();
        }
    }
}
