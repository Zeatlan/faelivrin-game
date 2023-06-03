using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    private TextMeshProUGUI text;

    void Start()
    {
        slider = GetComponent<Slider>();
        text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        text.SetText(health + "/" + health);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        text.SetText(health + "/" + slider.maxValue);
    }
}
