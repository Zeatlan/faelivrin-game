using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ArrowTranslator;
using static PhaseManager;

public class MouseController : MonoBehaviour
{
    public float speed;
    public CharacterInfo character;

    private PathFinder pathFinder;
    private RangeFinder rangeFinder;
    private ArrowTranslator arrowTranslator;

    private List<OverlayTile> path = new List<OverlayTile>();
    private List<OverlayTile> inRangeTiles = new List<OverlayTile>();
    private List<OverlayTile> previewedTiles = new List<OverlayTile>();

    [SerializeField] private PhaseManager phaseManager;
    [SerializeField] private CharacterSpawner characterSpawner;

    private bool isMoving = false;
    public bool isAtkMode = false;

    public void Start()
    {
        pathFinder = new PathFinder();
        rangeFinder = new RangeFinder();
        arrowTranslator = new ArrowTranslator();
        phaseManager = GameObject.Find("PhaseManager").GetComponent<PhaseManager>();
        characterSpawner = GameObject.Find("CharacterSpawner").GetComponent<CharacterSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D? focusedTileHit = GetFocusedOnTile();

        if (focusedTileHit.HasValue)
        {
            OverlayTile overlayTile = focusedTileHit.Value.collider.gameObject.GetComponent<OverlayTile>();
            transform.position = overlayTile.transform.position;
            gameObject.GetComponent<SpriteRenderer>().sortingOrder = overlayTile.GetComponent<SpriteRenderer>().sortingOrder;

            if (phaseManager.phaseState == Phase.Start)
            {
                if (overlayTile.isStartingTile)
                {
                    characterSpawner.DisplayPreview(overlayTile);
                }
                else
                {
                    characterSpawner.HidePreview();
                }
            }


            if (phaseManager.phaseState == Phase.PlayerTurn)
            {
                if (!isAtkMode && !isMoving && character.CanMove())
                {
                    GetInRangeTiles();
                    HandleArrowDisplay(overlayTile);
                }

            }
            HandleLeftLick(overlayTile);
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }
    }

    public void SwitchMode()
    {
        isAtkMode = !isAtkMode;

        if (character.CanAttack())
        {
            if (isAtkMode) GetAttackableTiles();
        }
        else
        {
            isAtkMode = false;
        }

        if (character.CanMove())
        {
            if (!isAtkMode) GetInRangeTiles();
        }
        else
        {
            isAtkMode = true;
            GetAttackableTiles();
        }
    }

    private void HandleArrowDisplay(OverlayTile overlayTile)
    {
        if (inRangeTiles.Contains(overlayTile) && !isMoving)
        {
            path = pathFinder.FindPath(character.activeTile, overlayTile, new List<OverlayTile>());

            foreach (OverlayTile tile in inRangeTiles)
            {
                tile.SetArrowSprite(ArrowDirection.None);
            }

            for (int i = 0; i < path.Count; i++)
            {
                OverlayTile previousTile = i > 0 ? path[i - 1] : character.activeTile;
                OverlayTile futureTile = i < path.Count - 1 ? path[i + 1] : null;

                ArrowDirection arrowDir = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                path[i].SetArrowSprite(arrowDir);
            }

            GetPreviewAttackableTiles(path[path.Count - 1]);
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
            if (phaseManager.phaseState == Phase.Start)
            {
                characterSpawner.SpawnCharacterOnTile(overlayTile);
            }

            if (phaseManager.phaseState == Phase.PlayerTurn)
            {
                if (character.CanMove() || character.CanAttack()) ClickOnMap(overlayTile);
                if (overlayTile.isBlocked && !overlayTile.isAttackableTile) ClickOnCharacter(overlayTile);
            }
        }
    }

    private void ClickOnMap(OverlayTile overlayTile)
    {

        if (inRangeTiles.Contains(overlayTile) && character.activeTile != overlayTile)
        {

            if (overlayTile.isAttackableTile && character.CanAttack())
            {
                AttackCharacterOnTile(overlayTile);
            }
            else if (!overlayTile.isBlocked && character.CanMove())
            {
                isMoving = true;
            }
        }
    }

    private void AttackCharacterOnTile(OverlayTile overlayTile)
    {
        CharacterInfo targetCharacter = MapManager.Instance.FindCharacterOnTile(overlayTile);

        if (targetCharacter && !MapManager.Instance.GetPlayerUnits().Contains(targetCharacter))
        {
            character.Attack(targetCharacter);
            phaseManager.PlayAction(character, ActionCharacter.Attack);
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

            if (MapManager.Instance.GetPlayerUnits().Contains(clickedCharacter) && (clickedCharacter.CanMove() || clickedCharacter.CanAttack()))
            {
                character = clickedCharacter;
                SwitchMode();
            }
        }
    }

    private void GetInRangeTiles()
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, character.stats.baseRange);

        foreach (OverlayTile item in inRangeTiles)
        {
            item.ShowTile();
        }
    }

    private void GetAttackableTiles()
    {
        ResetInRangeTile();

        inRangeTiles = rangeFinder.GetTilesInRange(character.activeTile, 1, true);

        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.ShowAttackableTile();
        }
    }

    private void GetPreviewAttackableTiles(OverlayTile startingTile)
    {
        ResetPreviewedTiles();

        previewedTiles = rangeFinder.GetTilesInRange(startingTile, 1, true);

        foreach (OverlayTile tile in previewedTiles)
        {
            tile.ShowPreviewAtackableTile();
        }
    }

    private void ResetPreviewedTiles()
    {
        List<OverlayTile> previewedTilesCopy = new List<OverlayTile>(previewedTiles);
        foreach (OverlayTile tile in previewedTilesCopy)
        {
            tile.HidePreview();
            previewedTiles.Remove(tile);
        }
    }

    private void ResetInRangeTile()
    {
        foreach (OverlayTile tile in inRangeTiles)
        {
            tile.HideTile();
        }
    }

    private void MoveAlongPath()
    {
        float step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;

        character.activeTile.isBlocked = false;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.0001f)
        {
            MapManager.Instance.PositionCharacterOnTile(path[0], character);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            phaseManager.PlayAction(character, ActionCharacter.Move);
            SwitchMode();
            if (previewedTiles.Count > 0) ResetPreviewedTiles();
            isMoving = false;
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
}
