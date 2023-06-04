using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private SpriteRenderer _sprite;

    [SerializeField] private float _leanDuration = 0.3f;
    [SerializeField] private float _maxLeanAngle = 15f;
    private LTDescr leanTweenDescription;

    public void TakeDamageAnim(CharacterInfo character)
    {
        _sprite = character.GetComponent<SpriteRenderer>();

        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, -_maxLeanAngle);

        if (leanTweenDescription != null)
        {
            LeanTween.cancel(leanTweenDescription.uniqueId);
        }

        leanTweenDescription = LeanTween.rotateZ(gameObject, -_maxLeanAngle, _leanDuration)
        .setEaseOutCubic()
        .setLoopPingPong(1)
        .setOnComplete(() =>
        {
            transform.rotation = originalRotation;
        });

        _sprite.color = new Color(1f, 0.5f, 0.5f, 1f);
        yield return new WaitForSeconds(_leanDuration);
        _sprite.color = new Color(1f, 1f, 1f, 1f);
    }

}
