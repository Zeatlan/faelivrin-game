using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BattleSystem
{
    public class UIManager : MonoBehaviour
    {
        [Header("Phase de placement")]
        [SerializeField] private Transform startingPhaseObj;
        [SerializeField] private Transform readyButton;
        [SerializeField] private Transform characterList;

        [Header("Tour du joueur")]
        [SerializeField] private Transform _playerPhaseObj;
        [SerializeField] private Transform _playerPhaseUI;
        [SerializeField] private TextMeshProUGUI _modeText;

        [Header("Tour de l'ennemie")]
        [SerializeField] private Transform _enemyPhaseObj;

        [Header("UI général")]
        [SerializeField] private Transform _unitPanel;
        [SerializeField] private Transform _victoryPhase;
        [SerializeField] private Transform _defeatPhase;

        [Header("Autres scripts")]
        [SerializeField] private MouseController _mouseController;

        private LTDescr _leanTweenDescription;

        void Awake()
        {
            LeanTween.init(1000);

            /*             readyButton.gameObject.SetActive(true);
                        startingPhaseObj.gameObject.SetActive(true);
                        characterList.gameObject.SetActive(true);
                        _playerPhaseObj.gameObject.SetActive(true);
                        _enemyPhaseObj.gameObject.SetActive(true);
                        _victoryPhase.gameObject.SetActive(true);
                        _defeatPhase.gameObject.SetActive(true); */
        }
    }
}