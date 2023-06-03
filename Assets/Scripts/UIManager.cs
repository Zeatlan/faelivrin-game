using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Transform startingPhaseObj;
    [SerializeField] private Transform readyButton;

    [SerializeField] private Transform playerPhaseObj;
    [SerializeField] private Transform playerPhaseUI;

    [SerializeField] private TextMeshProUGUI modeText;

    private MouseController mouseController;

    void Awake()
    {
        LeanTween.init(1000);

        mouseController = GameObject.Find("Cursor").GetComponent<MouseController>();

        startingPhaseObj = transform.Find("StartingPhase");
        ResetObjPos(startingPhaseObj);

        readyButton = transform.Find("ReadyButton");

        playerPhaseObj = transform.Find("PlayerPhase");
        ResetObjPos(playerPhaseObj);

        playerPhaseUI = transform.Find("PlayerPhaseUI");

        modeText = playerPhaseUI.Find("Action").GetChild(0).GetComponent<TextMeshProUGUI>();
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

    private IEnumerator PhaseMovementCoroutine(Transform obj)
    {
        ResetObjPos(obj);
        obj.LeanMoveLocal(new Vector3(0, 0, 0), 1).setEaseOutExpo().delay = 0.1f;
        yield return new WaitForSeconds(2f);
        obj.LeanMoveLocal(new Vector3(Screen.width, 0, 0), 2).setEaseOutExpo();
    }

    public void hideStartingUI()
    {
        StartCoroutine(HideStartingUICoroutine());
    }

    public void ShowPlayerPhaseUI()
    {
        playerPhaseUI.gameObject.SetActive(true);
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
        modeText.SetText(mouseController.isAtkMode ? "Deplacer" : "Attaquer");
    }
}
