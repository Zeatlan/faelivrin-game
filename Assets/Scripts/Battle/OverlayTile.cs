using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BattleSystem.ArrowTranslator;

namespace BattleSystem
{
    public class OverlayTile : MonoBehaviour
    {

        // A* pathfinding
        public int G;
        public int H;
        public int F { get { return G + H; } }

        public bool isBlocked;
        public bool isStartingTile;
        public bool isAttackableTile;

        public OverlayTile previousTile;

        public Vector3Int gridLocation;
        public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

        public List<Sprite> arrows;

        [SerializeField] private GameObject previewAttack;

        public void ShowTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }

        public void HideTile()
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
            SetArrowSprite(ArrowDirection.None);
            isStartingTile = false;
            isAttackableTile = false;
        }

        public void ShowStartingTile()
        {
            isStartingTile = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0.25f, 1, 0.25f, 1);
        }

        public void ShowAttackableTile()
        {
            isAttackableTile = true;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0.23f, 0.34f, 1);
        }

        public void ShowPreviewAtackableTile()
        {
            previewAttack.SetActive(true);
        }

        public void HidePreview()
        {
            previewAttack.SetActive(false);
        }

        public void SetBlocked(bool b)
        {
            isBlocked = b;
        }

        public void SetArrowSprite(ArrowDirection d)
        {
            SpriteRenderer arrow = GetComponentsInChildren<SpriteRenderer>()[1];

            if (d == ArrowDirection.None)
            {
                arrow.color = new Color(1, 1, 1, 0);
            }
            else
            {
                arrow.color = new Color(1, 1, 1, 1);
                arrow.sprite = arrows[(int)d];
                arrow.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
            }
        }
    }
}