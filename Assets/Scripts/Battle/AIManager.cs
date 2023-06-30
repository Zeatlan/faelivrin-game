using System.Collections;
using System.Collections.Generic;
using BattleSystem.Character;
using BattleSystem.Commands;
using UnityEngine;
using CharacterInfo = BattleSystem.Character.CharacterInfo;

namespace BattleSystem
{
    // Script qui permet de gérer la logique de l'intelligence artificielle
    public class AIManager : MonoBehaviour
    {
        private List<CharacterInfo> _playerUnits;

        [SerializeField] private OrderRecorder _orderRecorder;
        private MouseController _mouseController;

        private IOrder _moveOrder;
        private List<OverlayTile> _path = new List<OverlayTile>();
        private CharacterInfo _currentUnit;
        private PhaseManager _phaseManager;

        private Scenario _bestScenario;
        private TilesViewer _tilesViewer;

        private bool _isMoving = false;

        void Start()
        {
            _currentUnit = GetComponent<CharacterInfo>();
            _tilesViewer = new TilesViewer();
            _phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
            _mouseController = GameObject.Find("Cursor").GetComponent<MouseController>();
        }

        void Update()
        {
            if (PhaseManager.isGamePaused) return;

            if (_isMoving && _path.Count > 0)
            {
                _moveOrder.Execute();
            }
        }

        public void SetPlayerUnits(List<CharacterInfo> playerUnits)
        {
            _playerUnits = playerUnits;
        }

        /** <summary>
                <para>
                [IA AGRESSIVE]
                </para>
                Calcule le potentiel pour attaquer, plus ce potentiel est faible dans le scénario, plus il est tenté d'attaquer.
                Cette méthode se base sur la distance ainsi que la santé du joueur pour attaquer le plus faible et le plus proche.
            </summary>
            <param name="player">Quel joueur visons-nous</param>
        */
        private float CalculateAttackPotential(CharacterInfo player)
        {
            float potential = 0;

            float baseAttack = _currentUnit.character.stats.physicalDamage + _currentUnit.character.stats.magicalDamage;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            float playerHealth = player.character.stats.currentHealth;

            potential -= baseAttack * playerHealth;

            potential += distance;

            return potential;
        }

        /**
            <summary>
                Méthode appelée par les autres méthodes pour faire jouer l'IA.
            </summary>
        */
        public void IATurn()
        {
            List<Scenario> scenarios = new List<Scenario>();
            _orderRecorder = new OrderRecorder();

            // Calcul de tous les scénarios
            foreach (CharacterInfo player in _playerUnits)
            {
                Scenario scenario = new Scenario(
                    player,
                    Vector3.Distance(transform.position, player.transform.position),
                    player.character.stats.currentHealth
                );

                scenario.atkPotential = CalculateAttackPotential(player);

                scenarios.Add(scenario);
            }

            scenarios.Sort((s1, s2) =>
            {
                return s2.atkPotential.CompareTo(s1.atkPotential);
            });

            _bestScenario = scenarios[0];

            // Jouer le meilleur scénario
            if (_bestScenario.Distance <= _currentUnit.character.stats.atkRange)
            {
                _tilesViewer.GetAttackableTiles(_currentUnit);

                if (_tilesViewer.GetInRangeTiles().Contains(_bestScenario.PlayerUnit.activeTile))
                {
                    AttackPlayerUnit(_bestScenario.PlayerUnit);
                }
                else
                {
                    MoveTowardsPlayerUnit(_bestScenario.PlayerUnit);
                }
            }
            else
            {
                MoveTowardsPlayerUnit(_bestScenario.PlayerUnit);
            }
        }

        #region Actions toward player
        private void AttackPlayerUnit(CharacterInfo player)
        {
            IOrder attackOrder = new AttackOrder(_currentUnit.GetComponent<CharacterBase>(), player.GetComponent<CharacterBase>());
            _orderRecorder.AddOrder(attackOrder);
            attackOrder.Execute();
            _phaseManager.PlayAction(_currentUnit, ActionCharacter.Attack);

            _tilesViewer.ResetInRangeTile();
            _phaseManager.PlayAction(_currentUnit, ActionCharacter.Idle);
        }

        private void MoveTowardsPlayerUnit(CharacterInfo player)
        {
            List<OverlayTile> neighboursTiles = MapManager.Instance.GetNeighbourTiles(player.activeTile, new List<OverlayTile>());
            OverlayTile bestPlayerTile = neighboursTiles[0];
            OverlayTile bestTile = _currentUnit.activeTile;

            foreach (OverlayTile tile in neighboursTiles)
            {
                if (Vector3.Distance(transform.position, tile.gridLocation) < Vector3.Distance(transform.position, bestPlayerTile.gridLocation))
                {
                    bestPlayerTile = tile;
                }
            }

            _tilesViewer.GetInRangeTiles(_currentUnit);

            if (!_tilesViewer.GetInRangeTiles().Contains(bestPlayerTile))
            {
                foreach (OverlayTile tile in _tilesViewer.GetInRangeTiles())
                {
                    if (Vector3.Distance(tile.gridLocation, bestPlayerTile.gridLocation) < Vector3.Distance(bestTile.gridLocation, bestPlayerTile.gridLocation))
                    {
                        bestTile = tile;
                    }
                }
            }
            else
            {
                bestTile = bestPlayerTile;
            }

            PathFinder pathFinder = new PathFinder();
            _path = pathFinder.FindPath(_currentUnit.activeTile, bestTile, new List<OverlayTile>());

            _moveOrder = new MoveOrder(_currentUnit, _path, () =>
            {
                OnMovementFinished();
            });
            _orderRecorder.AddOrder(_moveOrder);
            _isMoving = true;
        }
        #endregion

        private void OnMovementFinished()
        {
            _isMoving = false;
            _phaseManager.PlayAction(_currentUnit, ActionCharacter.Move);
            _tilesViewer.ResetInRangeTile();

            _tilesViewer.GetAttackableTiles(_currentUnit);

            if (_tilesViewer.GetInRangeTiles().Contains(_bestScenario.PlayerUnit.activeTile))
            {
                AttackPlayerUnit(_bestScenario.PlayerUnit);
            }
            else
            {
                _phaseManager.PlayAction(_currentUnit, ActionCharacter.Idle);
            }
        }
    }
}