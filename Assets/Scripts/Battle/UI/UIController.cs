using System;
using System.Collections;
using System.Collections.Generic;
using BattleSystem.SO;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleSystem.UI
{
    public class UIController : MonoBehaviour
    {

        [SerializeField] private UIDocument _uiDocument;

        # region VAR phases
        private VisualElement _prepPhaseWindow;
        private VisualElement _playerPhaseWindow;
        private VisualElement _enemyPhaseWindow;
        private VisualElement _victoryPhaseWindow;
        private VisualElement _defeatPhaseWindow;
        #endregion

        #region VAR preparation phase
        private VisualElement _prepPhase;
        private Button _readyButton;
        #endregion

        #region VAR player phase
        // Visual Element (Parents)
        private GroupBox _playerActions; // Buttons parent

        // Buttons
        private Button _endCharacterTurnBtn;
        private Button _skillBtn;
        private Button _switchModeBtn;
        private Button _endTurnBtn;
        #endregion

        private VisualElement _characterBox; // Unit portrait (bottom left)

        [Header("External script")]
        [SerializeField] private PhaseManager _phaseManager;
        [SerializeField] private MouseController _mouseController;

        private VisualElement _root;

        private Tooltip _tooltip;

        void Awake()
        {
            LeanTween.init(1000);

            _root = _uiDocument.rootVisualElement;
            _tooltip = new Tooltip(_root);

            _prepPhaseWindow = _root.Q<VisualElement>("Phase__prep");
            _playerPhaseWindow = _root.Q<VisualElement>("Phase__player");
            _enemyPhaseWindow = _root.Q<VisualElement>("Phase__enemy");
            _victoryPhaseWindow = _root.Q<VisualElement>("Phase__victory");
            _defeatPhaseWindow = _root.Q<VisualElement>("Phase__defeat");

            InitializePrepPhase();
            InitializePlayerPhase();
        }

        #region initializers
        private void InitializePrepPhase()
        {
            _prepPhase = _root.Q<VisualElement>("Prep__phase");
            _readyButton = _root.Q<Button>("Button__ready");
            _readyButton.RegisterCallback<ClickEvent>(StartGame);
        }

        private void InitializePlayerPhase()
        {
            _playerActions = _root.Q<GroupBox>("Player__actions");
            _playerActions.style.display = DisplayStyle.None;

            _characterBox = _root.Q<VisualElement>("Character__box");
            _characterBox.style.display = DisplayStyle.None;

            _endCharacterTurnBtn = _root.Q<Button>("Action__idleCharacter");
            _endCharacterTurnBtn.RegisterCallback<ClickEvent>(EndCharacterTurn);

            _skillBtn = _root.Q<Button>("Action__skills");
            _skillBtn.RegisterCallback<ClickEvent>(UseSkill);
            _skillBtn.RegisterCallback<MouseEnterEvent>(ShowSkillTooltip);
            _skillBtn.RegisterCallback<MouseLeaveEvent>(HideSkillTooltip);

            _switchModeBtn = _root.Q<Button>("Action__switchMode");
            _switchModeBtn.RegisterCallback<ClickEvent>(SwitchMode);

            _endTurnBtn = _root.Q<Button>("Action__endTurn");
            _endTurnBtn.RegisterCallback<ClickEvent>(EndTurn);
        }


        #endregion

        private void StartGame(ClickEvent evt)
        {
            if (MapManager.Instance.GetPlayerUnits().Count > 0)
            {
                HideStartingUI();
                _phaseManager.StartTheGame();
            }
        }

        #region Phase transitions
        public void StartingPhaseAnim()
        {
            StartCoroutine(PhaseMovementCoroutine(_prepPhaseWindow));
        }

        public IEnumerator PlayerPhaseAnim()
        {
            yield return StartCoroutine(PhaseMovementCoroutine(_playerPhaseWindow));
        }

        public IEnumerator EnemyPhaseAnim()
        {
            yield return StartCoroutine(PhaseMovementCoroutine(_enemyPhaseWindow));
        }

        public IEnumerator VictoryPhaseAnim()
        {
            yield return StartCoroutine(PhaseMovementCoroutine(_victoryPhaseWindow));
        }

        public IEnumerator DefeatPhaseAnim()
        {
            yield return StartCoroutine(PhaseMovementCoroutine(_defeatPhaseWindow));
        }

        private void ResetPanelPosition(VisualElement phasePanel)
        {
            phasePanel.style.left = new Length(-100, LengthUnit.Percent);
        }

        private IEnumerator PhaseMovementCoroutine(VisualElement phasePanel)
        {
            PhaseManager.isGamePaused = true;
            ResetPanelPosition(phasePanel);

            var start = phasePanel.style.left;
            var mid = new Length(0, LengthUnit.Percent);
            var end = new Length(100, LengthUnit.Percent);

            float duration = 1.5f;

            yield return StartCoroutine(MoveBox(phasePanel, start.value, mid, duration / 2));

            yield return new WaitForSeconds(1.0f);

            yield return StartCoroutine(MoveBox(phasePanel, mid, end, duration / 2));

            PhaseManager.isGamePaused = false;
        }

        private IEnumerator MoveBox(VisualElement phasePanel, Length start, Length end, float duration)
        {
            float startTime = Time.time;

            while (Time.time - startTime < duration)
            {
                float timePassed = (Time.time - startTime) / duration;
                var newValue = Mathf.Lerp(start.value, end.value, timePassed);
                phasePanel.style.left = new Length(newValue, LengthUnit.Percent);

                yield return null;
            }

            phasePanel.style.left = end;
        }
        #endregion

        #region UI displayer
        public void HideStartingUI()
        {
            _prepPhase.style.display = DisplayStyle.None;
        }

        public void ShowPlayerPhaseUI()
        {
            _playerActions.style.display = DisplayStyle.Flex;
            _characterBox.style.display = DisplayStyle.Flex;
        }

        public void ShowEnemyPhaseUI()
        {
            _playerActions.style.display = DisplayStyle.None;
            _characterBox.style.display = DisplayStyle.Flex;
        }
        #endregion

        #region buttons handlers
        public void SetModeTextToAtk()
        {
            _switchModeBtn.text = "Attaquer";
        }

        public void SetModeTextToMove()
        {
            _switchModeBtn.text = "Déplacer";
        }

        public void SwitchMode(ClickEvent evt)
        {
            _mouseController.SwitchMode();

            if (_mouseController.character.CanMove() && !_mouseController.isAtkMode)
                SetModeTextToAtk();
            else if (_mouseController.character.CanAttack() && _mouseController.isAtkMode)
                SetModeTextToMove();
        }

        private void UseSkill(ClickEvent evt)
        {
            _mouseController.EnterSkillMode();
        }

        private void EndCharacterTurn(ClickEvent evt)
        {
            _phaseManager.EndCharacterTurn();
        }

        private void EndTurn(ClickEvent evt)
        {
            _phaseManager.EndTurn();
        }
        #endregion

        #region button hovered handlers
        private void ShowSkillTooltip(MouseEnterEvent evt)
        {
            Vector2 mousePosition = evt.mousePosition;
            AbilitySO playerAbility = _mouseController.character.GetStats().skill;
            _tooltip.ShowTooltip(mousePosition, playerAbility.name, playerAbility.description, playerAbility.icon);
        }

        private void HideSkillTooltip(MouseLeaveEvent evt)
        {
            _tooltip.HideTooltip();
        }
        #endregion
    }
}