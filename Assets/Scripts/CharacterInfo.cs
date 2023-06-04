using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct CharStats
{
    public int maxHealth;
    public int currentHealth;
    public int attack;
    public int range;
}

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public CharacterStatsSO stats;

    private UnitPanel _unitPanel;
    private Stats _statsUI;

    [SerializeField] private CharStats charStats;
    private bool _isPlayable;
    private CharacterAnimation _animation;

    void Start()
    {
        charStats.maxHealth = stats.baseHealth;
        charStats.currentHealth = stats.baseHealth;
        charStats.attack = stats.baseAttack;
        charStats.range = stats.baseRange;
        _isPlayable = true;
        _animation = GetComponent<CharacterAnimation>();
    }

    public void DisplayInfo()
    {
        _unitPanel = GameObject.Find("UnitPanel").GetComponent<UnitPanel>();
        _unitPanel.SetIcon(stats.icon);

        _unitPanel.SetName(stats.characterName);

        _unitPanel.SetMaxHealth(charStats.maxHealth);
        _unitPanel.SetHealth(charStats.currentHealth);

        _statsUI = GameObject.Find("Stats").GetComponent<Stats>();
        _statsUI.SetAttack(charStats.attack);
        _statsUI.SetRange(charStats.range);
    }

    public void Attack(CharacterInfo unit)
    {
        if (!_isPlayable) return;

        unit.TakeDamage(stats.baseAttack);
    }

    public void TakeDamage(int damage)
    {
        charStats.currentHealth -= damage;
        _animation.TakeDamageAnim(this);

        if (charStats.currentHealth < 0)
        {
            charStats.currentHealth = 0;
        }

        if (_unitPanel) _unitPanel.SetHealth(charStats.currentHealth);
    }

    public bool GetPlayable() { return _isPlayable; }

    public void SetPlayable(bool play)
    {
        _isPlayable = play;
        GetComponent<SpriteRenderer>().color = (_isPlayable) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
    }
}
