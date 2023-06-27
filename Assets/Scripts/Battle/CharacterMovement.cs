using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private List<Sprite> _swSprites; // South west
        [SerializeField] private List<Sprite> _seSprites; // South East
        [SerializeField] private List<Sprite> _neSprites; // North East
        [SerializeField] private List<Sprite> _nwSprites; // North West

        public enum CharacterDirection
        {
            NorthWest,
            NorthEast,
            SouthWest,
            SouthEast
        }

        public CharacterDirection Direction { get; set; }

        // Start is called before the first frame update
        void Start()
        {
            Direction = CharacterDirection.SouthWest;

        }

        // Update is called once per frame
        void Update()
        {
            List<Sprite> directionSprites = GetSpriteDirection();

            if (directionSprites != null)
            {
                _spriteRenderer.sprite = directionSprites[0];
            }
        }

        private List<Sprite> GetSpriteDirection()
        {
            List<Sprite> selectedSprites = null;

            switch (Direction)
            {
                case CharacterDirection.NorthWest:
                    selectedSprites = _nwSprites;
                    break;
                case CharacterDirection.NorthEast:
                    selectedSprites = _neSprites;
                    break;
                case CharacterDirection.SouthWest:
                    selectedSprites = _swSprites;
                    break;
                case CharacterDirection.SouthEast:
                    selectedSprites = _seSprites;
                    break;
            }

            return selectedSprites;
        }
    }
}