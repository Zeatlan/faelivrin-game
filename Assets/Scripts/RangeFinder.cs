using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RangeFinder
{

    public List<OverlayTile> GetTilesInRange(OverlayTile startingTile, int range, bool isAttacking = false)
    {
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < range)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach (OverlayTile tile in tileForPreviousStep)
            {
                surroundingTiles.AddRange(MapManager.Instance.GetNeighbourTiles(tile, new List<OverlayTile>(), isAttacking));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
    }

    public List<OverlayTile> GetSkillRange(OverlayTile startingTile, AbilitySO skill, Vector2Int direction)
    {
        List<OverlayTile> inRangeTiles = new List<OverlayTile>();
        int stepCount = 0;

        List<OverlayTile> tileForPreviousStep = new List<OverlayTile>();
        tileForPreviousStep.Add(startingTile);

        while (stepCount < skill.range)
        {
            List<OverlayTile> surroundingTiles = new List<OverlayTile>();

            foreach (OverlayTile tile in tileForPreviousStep)
            {
                surroundingTiles.AddRange(MapManager.Instance.GetNeighbourSkillTiles(tile, new List<OverlayTile>(), skill, direction));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
    }

}
