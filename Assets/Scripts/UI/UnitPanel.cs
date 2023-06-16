using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BattleSystem.UI
{
    public class UnitPanel : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _icon;

        public void SetMaxHealth(int health)
        {
            _slider.maxValue = health;
        }

        public void SetHealth(int health)
        {
            _slider.value = health;
            _healthText.SetText(health + "/" + _slider.maxValue);
        }

        public void SetName(string name)
        {
            _nameText.SetText(name);
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.sprite = sprite;
        }
    }
}