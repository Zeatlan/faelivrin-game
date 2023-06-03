using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterList;
    private CharacterInfo currentCharacter;
    private int cursor;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject previewCharacter;

    public void Start()
    {
        cursor = 0;
        SwitchCharacter(cursor);
        SwitchPreviewCharacter();
    }

    public void SwitchCharacter(int targetCursor)
    {
        characterPrefab = characterList[targetCursor];
    }

    public void SpawnCharacterOnTile(OverlayTile tile)
    {
        if (tile.isStartingTile)
        {
            if (currentCharacter != null)
            {
                MapManager.Instance.RemovePlayerUnit(currentCharacter);
                Destroy(currentCharacter.gameObject);
            }

            currentCharacter = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
            MapManager.Instance.PositionCharacterOnTile(tile, currentCharacter);
            MapManager.Instance.AddPlayerUnit(currentCharacter);
        }
    }

    private void SwitchPreviewCharacter()
    {
        if (characterPrefab)
        {
            if (previewCharacter) Destroy(previewCharacter);

            previewCharacter = Instantiate(characterPrefab);
            previewCharacter.name = "PreviewCharacter";
            previewCharacter.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            HidePreview();
        }
    }

    public void DisplayPreview(OverlayTile tile)
    {
        if (!previewCharacter) return;

        if (currentCharacter)
        {
            if (currentCharacter.activeTile == tile) return;
        }

        previewCharacter.SetActive(true);
        MapManager.Instance.PositionCharacterOnTile(tile, previewCharacter.GetComponent<CharacterInfo>());
    }

    public void HidePreview()
    {
        if (!previewCharacter) return;
        previewCharacter.SetActive(false);
    }

    public void SpawnEnemies(BattleMapSO battleMapData)
    {
        if (battleMapData.ennemiesSpawnPos.Count > 0)
        {
            Dictionary<Vector2Int, OverlayTile> map = MapManager.Instance.map;

            for (int i = 0; i < battleMapData.ennemies.Count; i++)
            {
                Vector2Int enemyCoordinate = new Vector2Int(battleMapData.ennemiesSpawnPos[i].x, battleMapData.ennemiesSpawnPos[i].y);

                if (map.ContainsKey(enemyCoordinate))
                {
                    CharacterInfo enemy = Instantiate(battleMapData.ennemies[i]).GetComponent<CharacterInfo>();
                    MapManager.Instance.PositionCharacterOnTile(map[enemyCoordinate], enemy);
                    MapManager.Instance.AddEnemyUnit(enemy);
                }
            }
        }
    }
}
