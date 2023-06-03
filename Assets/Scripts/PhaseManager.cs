using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private UIManager uiManager;
    private MouseController mouseController;

    // Start is called before the first frame update
    void Start()
    {
        phaseState = Phase.Start;
        uiManager.StartingPhaseAnim();
        mouseController = GameObject.Find("Cursor").GetComponent<MouseController>();
    }

    public void SwitchToPlayerTurn()
    {
        if (phaseState == Phase.Start)
        {
            if (MapManager.Instance.GetPlayerUnits().Count == 0) return;
            uiManager.hideStartingUI();
            MapManager.Instance.HideStartingTiles();
        }

        phaseState = Phase.PlayerTurn;
        mouseController.character = MapManager.Instance.GetPlayerUnits()[0];
        uiManager.PlayerPhaseAnim();
        uiManager.ShowPlayerPhaseUI();
        StartCoroutine(DisplayInfo());
    }

    private IEnumerator DisplayInfo()
    {
        yield return new WaitForSeconds(0.001f);
        mouseController.character.DisplayInfo();
    }
}
