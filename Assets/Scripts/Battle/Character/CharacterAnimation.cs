using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSystem.Character
{
    public class CharacterAnimation : MonoBehaviour
    {
        private SpriteRenderer _sprite;

        [SerializeField] private float _damageLeanDuration = 0.3f;
        [SerializeField] private float _maxDamageLeanAngle = -15f;

        [SerializeField] private float _jumpDuration = 0.3f;
        private float _maxJumpRange = 0.5f;

        [SerializeField] private float _dieLeanDuration = 0.7f;
        [SerializeField] private float _maxDieLeanAngle = -90f;
        private LTDescr _leanTweenDescription;

        #region Take Damage
        public void TakeDamageAnim(CharacterBase character)
        {
            _sprite = character.GetComponent<SpriteRenderer>();

            StartCoroutine(TakeDamageCoroutine());
        }

        private IEnumerator TakeDamageCoroutine()
        {
            Quaternion originalRotation = transform.rotation;
            Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, _maxDamageLeanAngle);

            if (_leanTweenDescription != null)
            {
                LeanTween.cancel(_leanTweenDescription.uniqueId);
            }

            _leanTweenDescription = LeanTween.rotateZ(gameObject, _maxDamageLeanAngle, _damageLeanDuration)
            .setEaseOutCubic()
            .setLoopPingPong(1)
            .setOnComplete(() =>
            {
                transform.rotation = originalRotation;
            });

            _sprite.color = new Color(1f, 0.5f, 0.5f, 1f);
            yield return new WaitForSeconds(_damageLeanDuration);
            _sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        #endregion

        #region Receive heal
        public void ReceiveHeal(CharacterBase character)
        {
            _sprite = character.GetComponent<SpriteRenderer>();

            StartCoroutine(ReceiveHealCoroutine());
        }

        public IEnumerator ReceiveHealCoroutine()
        {
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = initialPosition + new Vector3(0f, _maxJumpRange, 0);

            if (_leanTweenDescription != null)
            {
                LeanTween.cancel(_leanTweenDescription.uniqueId);
            }

            _leanTweenDescription = LeanTween.move(gameObject, targetPosition, _jumpDuration)
                .setEaseOutCubic()
                .setOnComplete(() =>
            {
                transform.position = targetPosition;
            });

            _sprite.color = new Color(0.3f, 1f, 0.3f, 1f);
            yield return new WaitForSeconds(_damageLeanDuration);

            _leanTweenDescription = LeanTween.move(gameObject, initialPosition, _jumpDuration)
                .setEaseOutCubic()
                .setOnComplete(() =>
            {
                transform.position = initialPosition;
            });
            _sprite.color = new Color(1f, 1f, 1f, 1f);
        }
        #endregion

        #region Die
        public void DieAnim(CharacterBase character)
        {
            _sprite = character.GetComponent<SpriteRenderer>();

            StartCoroutine(DieCoroutine());
        }

        private IEnumerator DieCoroutine()
        {
            if (_leanTweenDescription != null)
            {
                LeanTween.cancel(_leanTweenDescription.uniqueId);
            }

            _leanTweenDescription = LeanTween.rotateZ(gameObject, _maxDieLeanAngle, _dieLeanDuration)
                .setEaseOutCubic()
                .setOnComplete(() =>
                {
                    transform.rotation = Quaternion.Euler(0, 0, _maxDieLeanAngle);
                });

            yield return new WaitForSeconds(_dieLeanDuration);

            _sprite.color = new Color(0.2f, 0.2f, 0.2f, 0.7f);

            yield return new WaitForSeconds(1f);

            Destroy(_sprite.gameObject);
        }
        #endregion

    }
}