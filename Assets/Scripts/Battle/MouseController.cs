using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleSystem.Abilities;
using BattleSystem.Commands;
using BattleSystem.SO;
using BattleSystem.UI;
using UnityEngine;
using static BattleSystem.Abilities.AbilityHolder;
using static BattleSystem.ArrowTranslator;
using static BattleSystem.PhaseManager;
using CharacterInfo = BattleSystem.Character.CharacterInfo;

namespace BattleSystem
{
    public class MouseController : MonoBehaviour
    {
        public CharacterInfo character;

        private PathFinder _pathFinder;
        private RangeFinder _rangeFinder;
        private ArrowTranslator _arrowTranslator;

        private List<OverlayTile> _path = new List<OverlayTile>();

        [SerializeField] private PhaseManager _phaseManager;
        [SerializeField] private CharacterSpawner _characterSpawner;
        [SerializeField] private UIController _uiController;

        [SerializeField] private OrderRecorder _orderRecorder;
        private OverlayTile _clickedTile;
        private TilesViewer tilesViewer;

        public bool isMoving = false;
        public bool isAtkMode = false;
        private bool isSkillMode = false;
        private bool isSkillLineMode = false;
        private bool isDynamicSkill = false;
        private bool moveOrderInit = false;

        public void Start()
        {
            _pathFinder = new PathFinder();
            _rangeFinder = new RangeFinder();
            _arrowTranslator = new ArrowTranslator();

            _orderRecorder = new OrderRecorder();
            tilesViewer = new TilesViewer();
        }

        // Update is called once per frame
        void Update()
        {
            if (PhaseManager.isGamePaused) return;

            RaycastHit2D? focusedTileHit = GetFocusedOnTile();

            if (Input.GetKeyDown(KeyCode.Z))
            {
                _orderRecorder.UndoCommand();
            }

            if (focusedTileHit.HasValue)
            {
                OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
                transform.position = overlayTile.transform.position;
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

                if (_phaseManager.phaseState == Phase.Start)
                {
                    if (overlayTile.isStartingTile)
                    {
                        _characterSpawner.DisplayPreview(overlayTile);
                    }
                    else
                    {
                        _characterSpawner.HidePreview();
                    }
                }


                if (_phaseManager.phaseState == Phase.PlayerTurn)
                {
                    if (!isAtkMode && !isMoving && character.character.CanMove)
                    {
                        tilesViewer.GetInRangeTiles(character);
                        HandleArrowDisplay(overlayTile);
                    }

                    if (isSkillLineMode)
                    {
                        tilesViewer.PreviewSkillLine(character, this);
                    }

                    if (isDynamicSkill)
                    {
                        tilesViewer.PreviewDynamicSkill(character, this);
                    }

                }

                HandleLeftLick(overlayTile);

                if (_path.Count == 0 && moveOrderInit)
                {
                    moveOrderInit = false;
                    _phaseManager.PlayAction(character, ActionCharacter.Move);
                    SwitchMode();
                    _uiController.SwitchMode(new UnityEngine.UIElements.ClickEvent());
                    if (tilesViewer.GetPreviewedTiles().Count > 0) tilesViewer.ResetPreviewedTiles();
                    isMoving = false;
                }

                if (_path.Count > 0 && isMoving)
                {
                    IOrder moveOrder = new MoveOrder(character, _path);

                    if (!moveOrderInit)
                    {
                        _orderRecorder.AddOrder(moveOrder);
                        moveOrderInit = true;
                    }

                    moveOrder.Execute();
                }
            }

        }

        public void ResetMode()
        {
            tilesViewer.ResetInRangeTile();
            tilesViewer.ResetPreviewedTiles();

            isSkillLineMode = false;
            isDynamicSkill = false;
            isSkillMode = false;
            isAtkMode = (character.character.CanMove) ? false : true;

            if (isAtkMode) tilesViewer.GetAttackableTiles(character);
            else tilesViewer.GetInRangeTiles(character);
        }

        public void SwitchCharacter(CharacterInfo newCharacter)
        {
            character = newCharacter;
            ResetMode();
        }

        public void SwitchMode()
        {
            isSkillMode = false;
            isSkillLineMode = false;
            isDynamicSkill = false;
            isAtkMode = !isAtkMode;

            if (character.character.CanMove || character.character.CanAttack)
            {
                tilesViewer.ResetInRangeTile();
                tilesViewer.ResetPreviewedTiles();

                if (character.character.CanMove)
                {
                    if (!isAtkMode) tilesViewer.GetInRangeTiles(character);
                }
                else
                {
                    isAtkMode = true;
                }

                if (character.character.CanAttack)
                {
                    if (isAtkMode) tilesViewer.GetAttackableTiles(character);
                }
                else
                {
                    isAtkMode = false;
                    tilesViewer.GetInRangeTiles(character);
                }
            }
        }

        private void SwitchCharacter()
        {
            isSkillMode = false;
            isAtkMode = false;
            tilesViewer.ResetInRangeTile();
            tilesViewer.ResetPreviewedTiles();

            if (character.character.CanMove)
            {
                _uiController.SetModeTextToAtk();
                tilesViewer.GetInRangeTiles(character);
            }
            else
            {
                _uiController.SetModeTextToMove();
                tilesViewer.GetAttackableTiles(character);
            }
        }

        private void HandleArrowDisplay(OverlayTile overlayTile)
        {
            if (tilesViewer.GetInRangeTiles().Contains(overlayTile) && !isMoving)
            {
                _path = _pathFinder.FindPath(character.activeTile, overlayTile, new List<OverlayTile>());

                foreach (OverlayTile tile in tilesViewer.GetInRangeTiles())
                {
                    tile.SetArrowSprite(ArrowDirection.None);
                }

                for (int i = 0; i < _path.Count; i++)
                {
                    OverlayTile previousTile = i > 0 ? _path[i - 1] : character.activeTile;
                    OverlayTile futureTile = i < _path.Count - 1 ? _path[i + 1] : null;

                    ArrowDirection arrowDir = _arrowTranslator.TranslateDirection(previousTile, _path[i], futureTile);
                    _path[i].SetArrowSprite(arrowDir);
                }

                tilesViewer.GetPreviewAttackableTiles(_path[_path.Count - 1], character);
            }

        }

        private void HandleLeftLick(OverlayTile overlayTile)
        {
            if (Input.GetMouseButtonDown(0))
            {
#if UNITY_EDITOR
                Debug.LogFormat(
                    "{0} isBlocked({1}) isAttackable({2}) isStarting({3}) ",
                    overlayTile.gridLocation,
                    overlayTile.isBlocked,
                    overlayTile.isAttackableTile,
                    overlayTile.isStartingTile
                );
#endif
                if (_phaseManager.phaseState == Phase.Start)
                {
                    _characterSpawner.SpawnCharacterOnTile(overlayTile);
                }

                if (_phaseManager.phaseState == Phase.PlayerTurn)
                {
                    if (character.character.CanMove || character.character.CanAttack) ClickOnMap(overlayTile);
                    if (overlayTile.isBlocked && !overlayTile.isAttackableTile) ClickOnCharacter(overlayTile);
                }
            }
        }

        private void ClickOnMap(OverlayTile overlayTile)
        {
            if (tilesViewer.GetInRangeTiles().Contains(overlayTile) && character.activeTile != overlayTile)
            {
                if (isSkillMode && isDynamicSkill)
                {
                    character.gameObject.GetComponent<AbilityHolder>().UseSkillZone(tilesViewer.GetInRangeTiles());

                    ResetMode();
                }

                if (overlayTile.isAttackableTile && character.character.CanAttack)
                {
                    AttackCharacterOnTile(overlayTile);
                }
                else if (!overlayTile.isBlocked && character.character.CanMove)
                {
                    _clickedTile = overlayTile;
                    isMoving = true;
                }
            }
        }

        private void AttackCharacterOnTile(OverlayTile overlayTile)
        {
            CharacterInfo targetCharacter = MapManager.Instance.FindCharacterOnTile(overlayTile);

            if (targetCharacter && !MapManager.Instance.GetPlayerUnits().Contains(targetCharacter))
            {

                if (isSkillMode && character.gameObject.GetComponent<AbilityHolder>().CurrentState != AbilityState.ready) return;

                IOrder attackOrder = new AttackOrder(character.character, targetCharacter.character);
                _orderRecorder.AddOrder(attackOrder);
                _phaseManager.PlayAction(character, ActionCharacter.Attack);

                if (isSkillMode)
                {
                    if (isDynamicSkill)
                    {
                        character.gameObject.GetComponent<AbilityHolder>().UseSkillZone(tilesViewer.GetInRangeTiles());
                    }
                    else
                    {
                        character.gameObject.GetComponent<AbilityHolder>().UseSkill(overlayTile);
                    }

                    ResetMode();
                }

                if (targetCharacter.character.stats.currentHealth <= 0)
                {
                    overlayTile.isAttackableTile = false;
                    overlayTile.isBlocked = false;
                }
            }

            if (MapManager.Instance.GetPlayerUnits().Contains(targetCharacter))
            {
                ClickOnCharacter(overlayTile);
            }
        }

        private void ClickOnCharacter(OverlayTile overlayTile)
        {
            CharacterInfo clickedCharacter = MapManager.Instance.FindCharacterOnTile(overlayTile);

            if (clickedCharacter)
            {
                clickedCharacter.DisplayInfo();

                if (MapManager.Instance.GetPlayerUnits().Contains(clickedCharacter) && (clickedCharacter.character.CanMove || clickedCharacter.character.CanAttack))
                {
                    character = clickedCharacter;
                    SwitchCharacter();
                }
            }
        }

        public RaycastHit2D? GetFocusedOnTile()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2d = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2d, Vector2.zero);

            if (hits.Length > 0)
            {
                return hits.OrderByDescending(i => i.collider.transform.position.z).First();
            }

            return null;
        }

        public void EnterSkillMode()
        {
            AbilityHolder abilityHolder = character.GetComponent<AbilityHolder>();

            if (abilityHolder.CurrentState != AbilityState.ready) return;
            ResetMode();

            isSkillMode = (character.character.CanAttack) ? true : false;
            isAtkMode = true;

            AbilitySO userAbility = character.GetComponent<CharacterInfo>().character.stats.skill;
            tilesViewer.GetSkillTiles(character, userAbility);

            if (userAbility.rangeType == RangeType.Line)
            {
                isSkillLineMode = true;
            }
            else if (userAbility.zoneType == ZoneType.ZoneTarget)
            {
                isDynamicSkill = true;
            }
        }
    }
}