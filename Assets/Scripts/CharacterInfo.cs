using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CharStats
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

    [SerializeField] private CharStats _charStats;
    private bool _canAttack;
    private bool _canMove;
    private CharacterAnimation _animation;
    private float _speed = 3f;

    void Start()
    {
        _charStats.maxHealth = stats.baseHealth;
        _charStats.currentHealth = stats.baseHealth;
        _charStats.attack = stats.baseAttack;
        _charStats.range = stats.baseRange;
        _charStats.atkRange = stats.baseAtkRange;
        _canAttack = true;
        _canMove = true;
        _animation = GetComponent<CharacterAnimation>();
    }

    public void DisplayInfo()
    {
        _unitPanel = GameObject.Find("UnitPanel").GetComponent<UnitPanel>();
        _unitPanel.SetIcon(stats.icon);

        _unitPanel.SetName(stats.characterName);

        _unitPanel.SetMaxHealth(_charStats.maxHealth);
        _unitPanel.SetHealth(_charStats.currentHealth);

        _statsUI = GameObject.Find("Stats").GetComponent<Stats>();
        _statsUI.SetAttack(_charStats.attack);
        _statsUI.SetRange(_charStats.range);
        _statsUI.SetAtkRange(_charStats.atkRange);
    }

    public void Attack(CharacterInfo unit)
    {
        if (!_canAttack) return;

        unit.TakeDamage(stats.baseAttack);
    }

    public void TakeDamage(int damage)
    {
        _charStats.currentHealth -= damage;
        _animation.TakeDamageAnim(this);

        if (_charStats.currentHealth <= 0)
        {
            Die();
        }

        if (_unitPanel) _unitPanel.SetHealth(_charStats.currentHealth);
    }

    public void Move(List<OverlayTile> path)
    {
        float step = _speed * Time.deltaTime;
        float zIndex = path[0].transform.position.z;

        activeTile.isBlocked = false;
        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.0001f)
        {
            MapManager.Instance.PositionCharacterOnTile(path[0], this);
            path.RemoveAt(0);
        }
    }

    private void Die()
    {
        _charStats.currentHealth = 0;
        _animation.DieAnim(this);

        if (MapManager.Instance.GetEnemyUnits().Contains(this))
        {
            MapManager.Instance.RemoveEnemyUnit(this);
        }
        if (MapManager.Instance.GetPlayerUnits().Contains(this))
        {
            MapManager.Instance.RemovePlayerUnit(this);
        }
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

    public void SetActive()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void SetInactive()
    {
        if (!_canAttack && !_canMove)
        {
            GetComponent<SpriteRenderer>().color = (_canMove) ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, 0.7f);
        }
    }

    public CharStats GetStats() { return _charStats; }

    public void HealHealth(int amount)
    {
        _charStats.currentHealth += amount;
    }
}
