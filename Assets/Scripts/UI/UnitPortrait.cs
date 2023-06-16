using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPortrait : MonoBehaviour
{

    private const float ANIMATION_DURATION = 0.3f;
    private const float BASE_POS = 50f;
    private const float ELEVATED_POS = 75f;

    [SerializeField] private Image _border;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _icon;

    public bool isActive { get; set; }

    void Awake()
    {
        SetInactive();
    }

    public void SwitchActive()
    {
        CharacterSpawner characterSpawner = GameObject.Find("CharacterSpawner").GetComponent<CharacterSpawner>();
        characterSpawner.ChangeCharacterActiveUI(this);
    }

    public void SetActive()
    {
        _border.color = new Color(0.23f, 0.44f, 0.43f, 1f);
        _fill.color = new Color(0.27f, 0.66f, 0.44f, 1f);
        isActive = true;
        transform.LeanMoveLocalY(ELEVATED_POS, ANIMATION_DURATION);
    }

    public void SetInactive()
    {
        _border.color = new Color(0.11f, 0.13f, 0.37f, 1f);
        _fill.color = new Color(0.27f, 0.29f, 0.55f, 1f);
        isActive = false;
        transform.LeanMoveLocalY(BASE_POS, ANIMATION_DURATION);
    }

    public void SetIcon(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}
