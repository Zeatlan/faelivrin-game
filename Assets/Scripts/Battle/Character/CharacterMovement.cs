using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [SerializeField] private List<Sprite> _swSprites; // South west
        [SerializeField] private List<Sprite> _seSprites; // South East
        [SerializeField] private List<Sprite> _neSprites; // North East
        [SerializeField] private List<Sprite> _nwSprites; // North West

        private float _speed = 3f;

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

        public void Move(List<OverlayTile> path, CharacterInfo character)
        {
            float step = _speed * Time.deltaTime;
            float zIndex = path[0].transform.position.z;

            Vector3 targetPosition = path[0].transform.position;
            Vector3 direction = targetPosition - transform.position;

            character.activeTile.isBlocked = false;
            transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
            transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

            // Détermine la direction relative
            if (direction.x > 0 && direction.y > 0)
            {
                // Cible au nord-est
                Direction = CharacterDirection.NorthEast;
            }
            else if (direction.x < 0 && direction.y > 0)
            {
                // Cible au nord-ouest
                Direction = CharacterDirection.NorthWest;
            }
            else if (direction.x < 0 && direction.y < 0)
            {
                // Cible au sud-ouest
                Direction = CharacterDirection.SouthWest;
            }
            else if (direction.x > 0 && direction.y < 0)
            {
                // Cible au sud-est
                Direction = CharacterDirection.SouthEast;
            }

            if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
            {
                MapManager.Instance.PositionCharacterOnTile(path[0], character);
                path.RemoveAt(0);
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