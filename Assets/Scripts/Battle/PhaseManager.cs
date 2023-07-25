using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BattleSystem.UI;
using UnityEngine.SceneManagement;
using CharacterInfo = BattleSystem.Character.CharacterInfo;

namespace BattleSystem
{
    public class PhaseManager : MonoBehaviour
    {

        public enum Phase
        {
            Start = 1,
            PlayerTurn = 2,
            AllyTurn = 3,
            EnnemyTurn = 4,
            End = 5
        }

        public Phase phaseState;
        [SerializeField] private UIController _uiController;
        [SerializeField] private MouseController _mouseController;
        [SerializeField] private CharacterSpawner _characterSpawner;

        public static UnityEvent OnTurnEnded = new UnityEvent();
        public static bool isGamePaused = false;

        // Start is called before the first frame update
        void Start()
        {
            isGamePaused = true;
            phaseState = Phase.Start;
        }

        void Update()
        {
            if (phaseState != Phase.Start)
            {
                if (MapManager.Instance.GetPlayerUnits().Count == 0)
                {
                    StartCoroutine(playResult("defeat"));
                }

                if (MapManager.Instance.GetEnemyUnits().Count == 0)
                {
                    StartCoroutine(playResult("victory"));
                }
            }
        }

        private IEnumerator playResult(string result)
        {
            if (result.Equals("defeat"))
            {
                yield return StartCoroutine(_uiController.DefeatPhaseAnim());
            }
            else
            {
                yield return StartCoroutine(_uiController.VictoryPhaseAnim());
            }

            SceneManager.LoadScene("MainMenu");
        }

        private void RefillPlayableCharacter(List<CharacterInfo> characters)
        {
            foreach (CharacterInfo character in characters)
            {
                MapManager.Instance.AddPlayableUnit(character);
            }
        }

        public void StartTheGame()
        {
            StartCoroutine(SwitchToPlayerTurn());
        }

        public IEnumerator SwitchToPlayerTurn()
        {

            MapManager.Instance.HideAllTiles();
            if (phaseState == Phase.Start)
            {
                DestroyStartingPhase();
            }

            if (MapManager.Instance.GetPlayerUnits().Count == 0) yield break;

            phaseState = Phase.PlayerTurn;

            RefillPlayableCharacter(MapManager.Instance.GetPlayerUnits());

            foreach (CharacterInfo character in MapManager.Instance.GetPlayableUnits())
            {
                character.character.SetActive();
            }

            _mouseController.SwitchCharacter(MapManager.Instance.GetPlayableUnits()[0]);
            yield return StartCoroutine(_uiController.PlayerPhaseAnim());
            _uiController.ShowPlayerPhaseUI();
            yield return StartCoroutine(DisplayInfo());


            ResetActionOfEveryone();
            _mouseController.ResetMode();
        }

        public IEnumerator SwitchToEnemyTurn()
        {

            MapManager.Instance.HideAllTiles();
            OnTurnEnded.Invoke();
            if (MapManager.Instance.GetEnemyUnits().Count == 0) yield break;

            phaseState = Phase.EnnemyTurn;

            RefillPlayableCharacter(MapManager.Instance.GetEnemyUnits());
            yield return StartCoroutine(_uiController.EnemyPhaseAnim());
            _uiController.ShowEnemyPhaseUI();
            yield return StartCoroutine(DisplayInfo());

            ResetActionOfEveryone();

            foreach (CharacterInfo enemy in MapManager.Instance.GetPlayableUnits())
            {
                AIManager ai = enemy.GetComponent<AIManager>();
                ai.SetPlayerUnits(MapManager.Instance.GetPlayerUnits());
                ai.IATurn();
            }
        }

        private IEnumerator DisplayInfo()
        {
            yield return new WaitForSeconds(0.001f);
            _mouseController.character.DisplayInfo();
        }

        private void DestroyStartingPhase()
        {
            if (MapManager.Instance.GetPlayerUnits().Count == 0) return;
            _uiController.HideStartingUI();
            MapManager.Instance.HideStartingTiles();
            _characterSpawner.DestroyPreview();
        }

        private void SwitchPhase()
        {
            if (phaseState == Phase.PlayerTurn)
            {
                StartCoroutine(SwitchToEnemyTurn());
            }
            else if (phaseState == Phase.EnnemyTurn)
            {
                StartCoroutine(SwitchToPlayerTurn());
            }
        }

        private void ResetActionOfEveryone()
        {
            foreach (CharacterInfo character in MapManager.Instance.GetPlayableUnits())
            {
                character.character.CanAttack = true;
                character.character.CanMove = true;
            }
        }

        public void PlayAction(CharacterInfo character, ActionCharacter action)
        {
            switch (action)
            {
                case ActionCharacter.Attack:
                    character.character.CanAttack = false;
                    break;
                case ActionCharacter.Move:
                    character.character.CanMove = false;
                    break;
                case ActionCharacter.Idle:
                    character.character.CanAttack = false;
                    character.character.CanMove = false;
                    break;
                default:
                    return;
            }

            MapManager.Instance.HideAllTiles();

            if (!character.character.CanAttack && !character.character.CanMove)
                MapManager.Instance.RemovePlayableUnit(character);

            if (MapManager.Instance.GetPlayableUnits().Count == 0)
            {
                SwitchPhase();
            }
            else
            {

                if (!character.character.CanAttack && !character.character.CanMove)
                {
                    _mouseController.SwitchCharacter(MapManager.Instance.GetPlayableUnits()[0]);
                    character.DisplayInfo();
                }
            }
        }

        public void EndTurn()
        {
            List<CharacterInfo> playableUnitsCopy = new List<CharacterInfo>(MapManager.Instance.GetPlayableUnits());
            foreach (CharacterInfo playableUnit in playableUnitsCopy)
            {
                PlayAction(playableUnit, ActionCharacter.Idle);
            }
        }

        public void EndCharacterTurn()
        {
            if (_mouseController.isMoving) return;

            PlayAction(_mouseController.character, ActionCharacter.Idle);
            if (MapManager.Instance.GetPlayableUnits().Count > 0)
                _mouseController.SwitchCharacter(MapManager.Instance.GetPlayableUnits()[0]);
        }
    }
}