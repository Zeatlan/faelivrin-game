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

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.stats.baseRange);

        foreach (OverlayTile item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    public void GetAttackableTiles(CharacterInfo character)
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.stats.baseAtkRange, true);

        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    public void GetPreviewAttackableTiles(OverlayTile startingTile, CharacterInfo character)
    {
        ResetPreviewedTiles();

        previewedTiles = rangeFinder.GetTilesInRange(startingTile, character.stats.baseAtkRange, true);

        foreach (OverlayTile tile in previewedTiles)
        {
            tile.ShowPreviewAtackableTile();
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
