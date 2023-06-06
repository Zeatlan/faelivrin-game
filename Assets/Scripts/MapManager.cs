using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    private static MapManager _instance;
    public static MapManager Instance { get { return _instance; } }

    public OverlayTile overlayTilePrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    private List<OverlayTile> startingTiles;

    public BattleMapSO battleMapData;


    [Header("Test")]
    [SerializeField] private List<CharacterInfo> playerUnits;
    [SerializeField] private List<CharacterInfo> enemyUnits;
    [SerializeField] private List<CharacterInfo> playableUnits;

    private CharacterSpawner characterSpawner;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        startingTiles = new List<OverlayTile>();
        characterSpawner = GameObject.Find("CharacterSpawner").GetComponent<CharacterSpawner>();

        Tilemap tileMap = gameObject.GetComponentInChildren<Tilemap>();
        map = new Dictionary<Vector2Int, OverlayTile>();
        BoundsInt bounds = tileMap.cellBounds;

        for (int z = bounds.max.z; z > bounds.min.z; z--)
        {
            for (int y = bounds.min.y; y < bounds.max.y; y++)
            {
                for (int x = bounds.min.x; x < bounds.max.x; x++)
                {
                    CreateOverlayTile(x, y, z, tileMap);
                }
            }
        }

        characterSpawner.SpawnEnemies(battleMapData);
    }

    private void CreateOverlayTile(int x, int y, int z, Tilemap tileMap)
    {
        Vector3Int tileLocation = new Vector3Int(x, y, z);
        Vector2Int tileKey = new Vector2Int(x, y);

        if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
        {
            OverlayTile overlayTile = Instantiate(overlayTilePrefab, overlayContainer.transform);
            Vector3 cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);

            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
            overlayTile.GetComponent<SpriteRenderer>().sortingOrder = tileMap.GetComponent<TilemapRenderer>().sortingOrder;
            overlayTile.gridLocation = tileLocation;

            if (battleMapData.playerPossiblePos.Contains(overlayTile.gridLocation))
            {
                overlayTile.ShowStartingTile();
                startingTiles.Add(overlayTile);
            }

            map.Add(tileKey, overlayTile);
        }

    }

    public List<OverlayTile> GetNeighbourTiles(OverlayTile currentOverlayTile, List<OverlayTile> searchableTiles, bool isAttacking = false)
    {
        Dictionary<Vector2Int, OverlayTile> tileToSearch = new Dictionary<Vector2Int, OverlayTile>();

        if (searchableTiles.Count > 0)
        {
            foreach (OverlayTile item in searchableTiles)
            {
                tileToSearch.Add(item.grid2DLocation, item);
            }
        }
        else
        {
            tileToSearch = map;
        }

        List<OverlayTile> neighbours = new List<OverlayTile>();

        // Top
        LocationToCheck(tileToSearch, currentOverlayTile, neighbours, isAttacking, 0, 1);

        // Bottom
        LocationToCheck(tileToSearch, currentOverlayTile, neighbours, isAttacking, 0, -1);

        // Right
        LocationToCheck(tileToSearch, currentOverlayTile, neighbours, isAttacking, 1);

        // Left
        LocationToCheck(tileToSearch, currentOverlayTile, neighbours, isAttacking, -1);

        return neighbours;
    }

    private void LocationToCheck(
        Dictionary<Vector2Int, OverlayTile> tileToSearch,
        OverlayTile currentOverlayTile,
        List<OverlayTile> neighbours,
        bool isAttacking,
        int x = 0,
        int y = 0
    )
    {
        const int JUMP_HEIGHT = 1;

        Vector2Int locationToCheck = new Vector2Int(currentOverlayTile.gridLocation.x + x, currentOverlayTile.gridLocation.y + y);

        if (tileToSearch.ContainsKey(locationToCheck))
        {
            if (!isAttacking && tileToSearch[locationToCheck].isBlocked) return;

            if (Mathf.Abs(currentOverlayTile.gridLocation.z - tileToSearch[locationToCheck].gridLocation.z) <= JUMP_HEIGHT)
            {
                neighbours.Add(tileToSearch[locationToCheck]);
            }
        }

    }

    public void PositionCharacterOnTile(OverlayTile tile, CharacterInfo character)
    {
        if (character.activeTile) tile.SetBlocked(false);
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.activeTile = tile;
        tile.SetBlocked(true);
    }

    public List<CharacterInfo> GetPlayerUnits() { return playerUnits; }
    public void AddPlayerUnit(CharacterInfo unit) { playerUnits.Add(unit); }
    public void RemovePlayerUnit(CharacterInfo unit) { playerUnits.Remove(unit); }

    public List<CharacterInfo> GetEnemyUnits() { return enemyUnits; }
    public void AddEnemyUnit(CharacterInfo unit) { enemyUnits.Add(unit); }
    public void RemoveEnemyUnit(CharacterInfo unit) { enemyUnits.Remove(unit); }

    public List<CharacterInfo> GetPlayableUnits() { return playableUnits; }
    public void AddPlayableUnit(CharacterInfo unit) { playableUnits.Add(unit); }
    public void RemovePlayableUnit(CharacterInfo unit) { playableUnits.Remove(unit); }

    public void HideStartingTiles()
    {
        foreach (OverlayTile tile in startingTiles)
        {
            bool isTileAssignedToUnit = playerUnits.Any(unit => unit.activeTile == tile);

            tile.HideTile();
            if (!isTileAssignedToUnit) tile.SetBlocked(false);
        }
    }

    public void HideAllTiles()
    {
        foreach (KeyValuePair<Vector2Int, OverlayTile> kvp in map)
        {
            OverlayTile tile = kvp.Value;

            tile.HideTile();
        }
    }

    public CharacterInfo FindCharacterOnTile(OverlayTile overlayTile)
    {
        List<CharacterInfo> activeUnits = GetPlayerUnits();
        activeUnits.AddRange(GetEnemyUnits());

        foreach (CharacterInfo unit in activeUnits)
        {
            if (unit.activeTile == overlayTile) return unit;
        }

        return null;
    }
}
