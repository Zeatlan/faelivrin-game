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
        private Tooltip _tooltip;

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

        #region Groupboxs
        private GroupBox _atkGroup;
        private GroupBox _rangeGroup;
        private GroupBox _atkRangeGroup;
        #endregion


        void Start()
        {
            var root = hud.rootVisualElement;
            _tooltip = new Tooltip(root);

            _healthBar = root.Q<HealthBar>();

            _name = root.Q<Label>("Character__name");

            _icon = root.Q<IMGUIContainer>("Character__img");

            _atkGroup = root.Q<GroupBox>("Character__Atk");
            _atkGroup.RegisterCallback<MouseEnterEvent, string>(DisplayTooltip, "Attaque");
            _atkGroup.RegisterCallback<MouseLeaveEvent>(HideTooltip);

            _rangeGroup = root.Q<GroupBox>("Character__Range");
            _rangeGroup.RegisterCallback<MouseEnterEvent, string>(DisplayTooltip, "Portée");
            _rangeGroup.RegisterCallback<MouseLeaveEvent>(HideTooltip);

            _atkRangeGroup = root.Q<GroupBox>("Character__AtkRange");
            _atkRangeGroup.RegisterCallback<MouseEnterEvent, string>(DisplayTooltip, "Portée d'attaque");
            _atkRangeGroup.RegisterCallback<MouseLeaveEvent>(HideTooltip);

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

        #region setters
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
        #endregion

        #region Tooltip handler
        private void DisplayTooltip(MouseEnterEvent evt, string message)
        {
            Vector2 mousePosition = evt.mousePosition;

            _tooltip.SetOffset(0, 55);
            _tooltip.ShowTooltip(mousePosition, message);
            _tooltip.AutoSizeTooltip();
        }

        private void HideTooltip(MouseLeaveEvent evt)
        {
            _tooltip.HideTooltip();
        }
        #endregion
    }
}