using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private SpriteRenderer _sprite;

    [SerializeField] private float _damageLeanDuration = 0.3f;
    [SerializeField] private float _maxDamageLeanAngle = -15f;

    [SerializeField] private float _dieLeanDuration = 0.7f;
    [SerializeField] private float _maxDieLeanAngle = -90f;
    private LTDescr leanTweenDescription;

    public void TakeDamageAnim(CharacterInfo character)
    {
        _sprite = character.GetComponent<SpriteRenderer>();

        StartCoroutine(TakeDamageCoroutine());
    }

    private IEnumerator TakeDamageCoroutine()
    {
        Quaternion originalRotation = transform.rotation;
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, 0, _maxDamageLeanAngle);

        if (leanTweenDescription != null)
        {
            LeanTween.cancel(leanTweenDescription.uniqueId);
        }

        leanTweenDescription = LeanTween.rotateZ(gameObject, _maxDamageLeanAngle, _damageLeanDuration)
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

    public void DieAnim(CharacterInfo character)
    {
        _sprite = character.GetComponent<SpriteRenderer>();

        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        if (leanTweenDescription != null)
        {
            LeanTween.cancel(leanTweenDescription.uniqueId);
        }

        leanTweenDescription = LeanTween.rotateZ(gameObject, _maxDieLeanAngle, _dieLeanDuration)
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

}
