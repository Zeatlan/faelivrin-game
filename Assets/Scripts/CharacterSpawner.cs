using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.SO;
using BattleSystem.UI;
using UnityEngine;

namespace BattleSystem
{
    public class CharacterSpawner : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _characterList;

        private CharacterInfo _currentCharacter;
        private int _cursor;

        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private GameObject _previewCharacter;

        [Header("UI")]
        [SerializeField] private Transform _characterListUI;
        [SerializeField] private GameObject _characterCardPrefab;
        private List<UnitPortrait> _unitPortraits;

        public void Start()
        {
            _unitPortraits = new List<UnitPortrait>();
            UpdateCursor(0);
            GenerateCharacterListUI();
        }

        public void SwitchCharacter(int targetCursor)
        {
            _characterPrefab = _characterList[targetCursor];
        }

        public void SpawnCharacterOnTile(OverlayTile tile)
        {

            if (tile.isStartingTile)
            {

                if (MapManager.Instance.FindCharacterOnTile(tile) != null) return;

                _currentCharacter = Instantiate(_characterPrefab).GetComponent<CharacterInfo>();
                CharacterInfo existingCharacter = MapManager.Instance.GetPlayerUnitByName(_currentCharacter.gameObject.name);

                if (existingCharacter != null)
                {
                    MapManager.Instance.RemovePlayerUnit(existingCharacter);
                    Destroy(existingCharacter.gameObject);
                }
                MapManager.Instance.PositionCharacterOnTile(tile, _currentCharacter);
                MapManager.Instance.AddPlayerUnit(_currentCharacter);
            }
        }


        private void GenerateCharacterListUI()
        {
            foreach (GameObject character in _characterList)
            {
                UnitPortrait newCard = Instantiate(_characterCardPrefab, _characterListUI).GetComponent<UnitPortrait>();
                newCard.SetIcon(character.GetComponent<CharacterInfo>().stats.icon);
                _unitPortraits.Add(newCard);
            }

            _unitPortraits[0].SetActive();
        }

        private void UpdateCursor(int newCursor)
        {
            _cursor = newCursor;
            SwitchCharacter(_cursor);
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
            if (_characterPrefab)
            {
                if (_previewCharacter) Destroy(_previewCharacter);

                _previewCharacter = Instantiate(_characterPrefab);
                _previewCharacter.name = "PreviewCharacter";
                _previewCharacter.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
                HidePreview();
            }
        }

        public void DisplayPreview(OverlayTile tile)
        {
            if (!_previewCharacter) return;

            if (_currentCharacter)
            {
                if (_currentCharacter.activeTile == tile || MapManager.Instance.FindCharacterOnTile(tile))
                {
                    HidePreview();
                    return;
                }
            }

            _previewCharacter.SetActive(true);
            MapManager.Instance.PositionCharacterOnTile(tile, _previewCharacter.GetComponent<CharacterInfo>());
        }

        public void HidePreview()
        {
            if (!_previewCharacter) return;
            _previewCharacter.SetActive(false);
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
                        AIManager aiManager = enemy.gameObject.AddComponent<AIManager>();
                        MapManager.Instance.PositionCharacterOnTile(map[enemyCoordinate], enemy);
                        MapManager.Instance.AddEnemyUnit(enemy);
                    }
                }
            }
        }

        public void DestroyPreview()
        {
            Destroy(_previewCharacter);
        }
    }
}