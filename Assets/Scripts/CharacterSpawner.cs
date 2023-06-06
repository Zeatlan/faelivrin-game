using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> characterList;

    private CharacterInfo currentCharacter;
    private int cursor;

    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject previewCharacter;

    [Header("UI")]
    [SerializeField] private Transform characterListUI;
    [SerializeField] private GameObject characterCardPrefab;
    private List<UnitPortrait> _unitPortraits;

    public void Start()
    {
        _unitPortraits = new List<UnitPortrait>();
        UpdateCursor(0);
        GenerateCharacterListUI();
    }

    public void SwitchCharacter(int targetCursor)
    {
        characterPrefab = characterList[targetCursor];
    }

    public void SpawnCharacterOnTile(OverlayTile tile)
    {

        if (tile.isStartingTile)
        {

            if (MapManager.Instance.FindCharacterOnTile(tile) != null) return;

            currentCharacter = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
            CharacterInfo existingCharacter = MapManager.Instance.GetPlayerUnitByName(currentCharacter.gameObject.name);

            if (existingCharacter != null)
            {
                MapManager.Instance.RemovePlayerUnit(existingCharacter);
                Destroy(existingCharacter.gameObject);
            }

            MapManager.Instance.PositionCharacterOnTile(tile, currentCharacter);
            MapManager.Instance.AddPlayerUnit(currentCharacter);
            MapManager.Instance.AddPlayableUnit(currentCharacter);
        }
    }


    private void GenerateCharacterListUI()
    {
        foreach (GameObject character in characterList)
        {
            UnitPortrait newCard = Instantiate(characterCardPrefab, characterListUI).GetComponent<UnitPortrait>();
            newCard.SetIcon(character.GetComponent<CharacterInfo>().stats.icon);
            _unitPortraits.Add(newCard);
        }

        _unitPortraits[0].SetActive();
    }

    private void UpdateCursor(int newCursor)
    {
        cursor = newCursor;
        SwitchCharacter(cursor);
        SwitchPreviewCharacter();
    }

    public void ChangeCharacterActiveUI(UnitPortrait selectedCard)
    {
        for (int i = 0; i < _unitPortraits.Count; i++)
        {

            if (_unitPortraits[i].isActive) _unitPortraits[i].SetInactive();

            if (_unitPortraits[i] == selectedCard)
            {
                UpdateCursor(i);
            }
        }

        selectedCard.SetActive();
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
            if (currentCharacter.activeTile == tile || MapManager.Instance.FindCharacterOnTile(tile))
            {
                HidePreview();
                return;
            }
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

    public void DestroyPreview()
    {
        Destroy(previewCharacter);
    }
}
