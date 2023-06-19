using System.Collections;
using System.Collections.Generic;
using BattleSystem.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace BattleSystem.UI
{
    public class UnitPanel : MonoBehaviour
    {
        public UIDocument hud;

        #region heathBar
        private HealthBar _healthBar;
        public int _currentHealth;
        private int _maxHealth;

        [Range(0, 1)]
        public float healthPercent = 1;
        #endregion

        private Label _name;

        private IMGUIContainer _icon;

        #region stats
        private Label _atk;
        private Label _range;
        private Label _atkRange;
        #endregion
        void Start()
        {
            var root = hud.rootVisualElement;

            _healthBar = root.Q<HealthBar>();

            _name = root.Q<Label>("Character__name");

            _icon = root.Q<IMGUIContainer>("Character__img");

            _atk = root.Q<Label>("Atk__value");
            _range = root.Q<Label>("Range__value");
            _atkRange = root.Q<Label>("AtkRange__value");

            _healthBar.value = 1;
        }

        public void InitializePanel(CharacterInfo character)
        {
            SetHealth(character.GetStats().currentHealth);
            SetMaxHealth(character.GetStats().maxHealth);
            SetName(character.stats.characterName);
            SetIcon(character.stats.icon);
            SetAttack(character.GetStats().attack);
            SetRange(character.GetStats().range);
            SetAttackRange(character.GetStats().atkRange);

            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            _healthBar.MaxHealth = _maxHealth;
            _healthBar.CurrentHealth = _currentHealth;
            _healthBar.UpdateHealthDisplay();

            int currentHealthValue = (int)(healthPercent * _currentHealth);
            _healthBar.value = (float)currentHealthValue / _maxHealth;
        }

        private void OnValidate()
        {
            if (_healthBar != null)
            {
                UpdateHealthBar();
            }
        }

        public void SetMaxHealth(int health)
        {
            _maxHealth = health;
            UpdateHealthBar();
        }

        public void SetHealth(int health)
        {
            _currentHealth = health;
            UpdateHealthBar();
        }

        public void SetName(string name)
        {
            _name.text = name;
        }

        public void SetIcon(Sprite sprite)
        {
            _icon.style.backgroundImage = new StyleBackground(sprite);
        }

        public void SetAttack(int atk)
        {
            _atk.text = atk.ToString();
        }

        public void SetRange(int range)
        {
            _range.text = range.ToString();
        }

        public void SetAttackRange(int atkRange)
        {
            _atkRange.text = atkRange.ToString();
        }

    }
}