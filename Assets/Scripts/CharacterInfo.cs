using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile activeTile;
    public CharacterStatsSO stats;

    [SerializeField] private int currentHealth;

    private UnitPanel unitPanel;
    private Stats statsUI;

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
        unitPanel = GameObject.Find("UnitPanel").GetComponent<UnitPanel>();
        unitPanel.SetIcon(stats.icon);

        unitPanel.SetName(stats.characterName);

        currentHealth = stats.baseHealth;

        unitPanel.SetMaxHealth(stats.baseHealth);

        statsUI = GameObject.Find("Stats").GetComponent<Stats>();
        statsUI.SetAttack(stats.baseAttack);
        statsUI.SetRange(stats.baseRange);
    }

    public void Attack(CharacterInfo unit)
    {
        unit.TakeDamage(stats.baseAttack);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        unitPanel.SetHealth(currentHealth);
    }
}
