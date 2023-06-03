using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitPanel : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image icon;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        healthText.SetText(health + "/" + health);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        healthText.SetText(health + "/" + slider.maxValue);
    }

    public void SetName(string name)
    {
        nameText.SetText(name);
    }

    public void SetIcon(Sprite sprite)
    {
        icon.sprite = sprite;
    }
}
