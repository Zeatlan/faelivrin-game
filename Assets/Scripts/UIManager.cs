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
    [SerializeField] private Transform playerPhaseObj;
    [SerializeField] private Transform playerPhaseUI;
    [SerializeField] private TextMeshProUGUI modeText;

    [Header("Tour de l'ennemie")]
    [SerializeField] private Transform enemyPhaseObj;

    [Header("UI général")]
    [SerializeField] private Transform unitPanel;

    [Header("Autres scripts")]
    [SerializeField] private MouseController mouseController;

    private LTDescr leanTweenDescription;

    void Awake()
    {
        LeanTween.init(1000);

        readyButton.gameObject.SetActive(true);
        startingPhaseObj.gameObject.SetActive(true);
        characterList.gameObject.SetActive(true);
        playerPhaseObj.gameObject.SetActive(true);
        enemyPhaseObj.gameObject.SetActive(true);

        ResetObjPos(startingPhaseObj);
        ResetObjPos(playerPhaseObj);
        ResetObjPos(enemyPhaseObj);
    }

    private void ResetObjPos(Transform obj)
    {
        obj.localPosition = new Vector2(-Screen.width, 0);
    }

    public void StartingPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(startingPhaseObj));
    }

    public void PlayerPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(playerPhaseObj));
    }

    public void EnemyPhaseAnim()
    {
        StartCoroutine(PhaseMovementCoroutine(enemyPhaseObj));
    }

    private IEnumerator PhaseMovementCoroutine(Transform obj)
    {
        ResetObjPos(obj);
        yield return new WaitForSeconds(0.001f);

        leanTweenDescription = LeanTween.moveLocal(obj.gameObject, new Vector3(0, 0, 0), 1)
            .setEaseOutExpo()
            .setOnComplete(() =>
            {
                LeanTween.moveLocal(obj.gameObject, new Vector3(Screen.width, 0, 0), 1).setDelay(1).setEaseOutExpo();
            }
        );
    }

    public void hideStartingUI()
    {
        StartCoroutine(HideStartingUICoroutine());
    }

    public void ShowPlayerPhaseUI()
    {
        playerPhaseUI.gameObject.SetActive(true);
        unitPanel.gameObject.SetActive(true);
    }

    public void ShowEnemyPhaseUI()
    {
        playerPhaseUI.gameObject.SetActive(false);
        unitPanel.gameObject.SetActive(true);
    }

    private IEnumerator HideStartingUICoroutine()
    {
        readyButton.LeanMoveLocalY(Screen.height, 2f).setEaseOutExpo().delay = 0.1f;
        yield return new WaitForSeconds(2f);
        readyButton.gameObject.SetActive(false);
    }

    public void SwitchMode()
    {
        mouseController.SwitchMode();
        if (!mouseController.hasMoved)
            modeText.SetText(mouseController.isAtkMode ? "Deplacer" : "Attaquer");
    }
}
