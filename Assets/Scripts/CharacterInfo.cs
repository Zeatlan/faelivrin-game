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

    void Start()
    {
        charStats.maxHealth = stats.baseHealth;
        charStats.currentHealth = stats.baseHealth;
        charStats.attack = stats.baseAttack;
        charStats.range = stats.baseRange;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            //Attack(this);
            Debug.Log(stats.characterName + " - " + activeTile.isBlocked);
        }
    }

    public void DisplayInfo()
    {
        Debug.Log(charStats.currentHealth);
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
        Debug.Log(stats.characterName + " is attacking " + unit.stats.characterName);
        unit.TakeDamage(stats.baseAttack);
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(stats.characterName + " took " + damage + " damage");
        charStats.currentHealth -= damage;

        if (charStats.currentHealth < 0)
        {
            charStats.currentHealth = 0;
        }

        if (_unitPanel) _unitPanel.SetHealth(charStats.currentHealth);
    }
}
