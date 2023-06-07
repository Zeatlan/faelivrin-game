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
    public int atkRange;
}

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public CharacterStatsSO stats;

    private UnitPanel _unitPanel;
    private Stats _statsUI;

    [SerializeField] private CharStats charStats;
    private bool _canAttack;
    private bool _canMove;
    private CharacterAnimation _animation;

    void Start()
    {
        charStats.maxHealth = stats.baseHealth;
        charStats.currentHealth = stats.baseHealth;
        charStats.attack = stats.baseAttack;
        charStats.range = stats.baseRange;
        charStats.atkRange = stats.baseAtkRange;
        _canAttack = true;
        _canMove = true;
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
        _statsUI.SetAtkRange(charStats.atkRange);
    }

    public void Attack(CharacterInfo unit)
    {
        if (!_canAttack) return;

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

    public bool CanAttack() { return _canAttack == true; }

    public void SetCanAttack(bool atk)
    {
        _canAttack = atk;
        SetInactive();
    }

    public bool CanMove() { return _canMove == true; }

    public void SetCanMove(bool move)
    {
        _canMove = move;
        SetInactive();
    }

    public void SetInactive()
    {
        if (!_canAttack && !_canMove)
        {
            GetComponent<SpriteRenderer>().color = (_canMove) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
        }
    }
}
