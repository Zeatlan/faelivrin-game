using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

        readyButton.gameObject.SetActive(true);
        startingPhaseObj.gameObject.SetActive(true);
        characterList.gameObject.SetActive(true);
        _playerPhaseObj.gameObject.SetActive(true);
        _enemyPhaseObj.gameObject.SetActive(true);
        _victoryPhase.gameObject.SetActive(true);
        _defeatPhase.gameObject.SetActive(true);

        ResetObjPos(startingPhaseObj);
        ResetObjPos(_playerPhaseObj);
        ResetObjPos(_enemyPhaseObj);
        ResetObjPos(_victoryPhase);
        ResetObjPos(_defeatPhase);
    }

    private void ResetObjPos(Transform obj)
    {
        obj.localPosition = new Vector2(-Screen.width, 0);
    }

    public void StartingPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(startingPhaseObj));
    }

    public IEnumerator PlayerPhaseAnim()
    {
        yield return StartCoroutine(PhaseMovementCoroutine(_playerPhaseObj));
    }

    public IEnumerator EnemyPhaseAnim()
    {
        yield return StartCoroutine(PhaseMovementCoroutine(_enemyPhaseObj));
    }

    public void VictoryPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(_victoryPhase));
    }

    public void DefeatPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(_defeatPhase));
    }

    private IEnumerator PhaseMovementCoroutine(Transform obj)
    {
        PhaseManager.isGamePaused = true;
        ResetObjPos(obj);

        yield return new WaitForSeconds(0.001f);

        _leanTweenDescription = LeanTween.moveLocal(obj.gameObject, new Vector3(0, 0, 0), 1)
            .setEaseOutExpo()
            .setOnComplete(() =>
            {
                LeanTween.moveLocal(obj.gameObject, new Vector3(Screen.width, 0, 0), 1).setDelay(1).setEaseOutExpo();
                PhaseManager.isGamePaused = false;
            }
        );
    }

    public void hideStartingUI()
    {
        StartCoroutine(HideStartingUICoroutine());
    }

    public void ShowPlayerPhaseUI()
    {
        _playerPhaseUI.gameObject.SetActive(true);
        _unitPanel.gameObject.SetActive(true);
    }

    public void ShowEnemyPhaseUI()
    {
        _playerPhaseUI.gameObject.SetActive(false);
        _unitPanel.gameObject.SetActive(true);
    }

    private IEnumerator HideStartingUICoroutine()
    {
        characterList.LeanMoveLocalY(-Screen.height, 2f).setEaseOutExpo().delay = 0.1f;
        readyButton.LeanMoveLocalY(Screen.height, 2f).setEaseOutExpo().delay = 0.1f;
        yield return new WaitForSeconds(2f);
        readyButton.gameObject.SetActive(false);
        characterList.gameObject.SetActive(false);
    }

    public void SwitchMode()
    {
        _mouseController.SwitchMode();

        if (_mouseController.character.CanMove() && !_mouseController.isAtkMode)
            SetModeTextToAtk();
        else if (_mouseController.character.CanAttack() && _mouseController.isAtkMode)
            SetModeTextToMove();
    }

    public void SetModeTextToAtk()
    {
        _modeText.SetText("Attaquer");
    }

    public void SetModeTextToMove()
    {
        _modeText.SetText("Déplacer");
    }
}
