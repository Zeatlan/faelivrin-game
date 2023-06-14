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
    [SerializeField] private MouseController mouseController;
    [SerializeField] private CharacterSpawner characterSpawner;

    // Start is called before the first frame update
    void Start()
    {
        phaseState = Phase.Start;
        uiManager.StartingPhaseAnim();
    }

    void Update()
    {
        if (phaseState != Phase.Start)
        {
            if (MapManager.Instance.GetPlayerUnits().Count == 0)
            {
                uiManager.DefeatPhaseAnim();
            }

            if (MapManager.Instance.GetEnemyUnits().Count == 0)
            {
                uiManager.VictoryPhaseAnim();
            }
        }
    }

    private void RefillPlayableCharacter(List<CharacterInfo> characters)
    {
        foreach (CharacterInfo character in characters)
        {
            MapManager.Instance.AddPlayableUnit(character);
        }
    }

    public void SwitchToPlayerTurn()
    {
        MapManager.Instance.HideAllTiles();
        if (phaseState == Phase.Start)
        {
            DestroyStartingPhase();
        }

        if (MapManager.Instance.GetPlayerUnits().Count == 0) return;

        phaseState = Phase.PlayerTurn;

        RefillPlayableCharacter(MapManager.Instance.GetPlayerUnits());
        mouseController.character = MapManager.Instance.GetPlayerUnits()[0];
        uiManager.PlayerPhaseAnim();
        uiManager.ShowPlayerPhaseUI();
        StartCoroutine(DisplayInfo());


        ResetActionOfEveryone();
        mouseController.ResetMode();
    }

    public void SwitchToEnemyTurn()
    {
        MapManager.Instance.HideAllTiles();
        if (MapManager.Instance.GetEnemyUnits().Count == 0) return;

        phaseState = Phase.EnnemyTurn;

        RefillPlayableCharacter(MapManager.Instance.GetEnemyUnits());
        uiManager.EnemyPhaseAnim();
        uiManager.ShowEnemyPhaseUI();
        StartCoroutine(DisplayInfo());

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
        mouseController.character.DisplayInfo();
    }

    private void DestroyStartingPhase()
    {
        if (MapManager.Instance.GetPlayerUnits().Count == 0) return;
        uiManager.hideStartingUI();
        MapManager.Instance.HideStartingTiles();
        characterSpawner.DestroyPreview();
    }

    private void SwitchPhase()
    {
        if (phaseState == Phase.PlayerTurn)
        {
            SwitchToEnemyTurn();
        }
        else if (phaseState == Phase.EnnemyTurn)
        {
            SwitchToPlayerTurn();
        }
    }

    private void ResetActionOfEveryone()
    {
        foreach (CharacterInfo character in MapManager.Instance.GetPlayableUnits())
        {
            character.SetCanAttack(true);
            character.SetCanMove(true);
        }
    }

    public void PlayAction(CharacterInfo character, ActionCharacter action)
    {
        switch (action)
        {
            case ActionCharacter.Attack:
                character.SetCanAttack(false);
                break;
            case ActionCharacter.Move:
                character.SetCanMove(false);
                break;
            case ActionCharacter.Idle:
                character.SetCanAttack(false);
                character.SetCanMove(false);
                MapManager.Instance.RemovePlayableUnit(character);
                break;
            default:
                return;
        }

        MapManager.Instance.HideAllTiles();

        if (!character.CanAttack() && !character.CanMove())
            MapManager.Instance.RemovePlayableUnit(character);

        if (MapManager.Instance.GetPlayableUnits().Count == 0)
        {
            SwitchPhase();
        }
        else
        {

            if (!character.CanAttack() && !character.CanMove())
            {
                mouseController.character = MapManager.Instance.GetPlayableUnits()[0];
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
        if (mouseController.isMoving) return;

        PlayAction(mouseController.character, ActionCharacter.Idle);
        if (MapManager.Instance.GetPlayableUnits().Count > 0)
            mouseController.SwitchCharacter(MapManager.Instance.GetPlayableUnits()[0]);
    }
}
