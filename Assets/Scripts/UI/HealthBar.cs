using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleSystem.UI
{
    public class HealthBar : VisualElement, INotifyValueChanged<float>
    {
        public int width { get; set; }
        public int height { get; set; }

        private float _value;
        public float value
        {
            get => _value; set
            {
                if (EqualityComparer<float>.Default.Equals(_value, value))
                    return;
                if (this.panel != null)
                {
                    using (ChangeEvent<float> pooled = ChangeEvent<float>.GetPooled(this._value, value))
                    {
                        pooled.target = (IEventHandler)this;
                        this.SetValueWithoutNotify(value);
                        this.SendEvent((EventBase)pooled);
                    }
                }
                else
                {
                    SetValueWithoutNotify(value);
                }
            }
        }

        public void SetValueWithoutNotify(float newValue)
        {
            _value = newValue;
        }

        public enum FillType
        {
            Horizontal,
            Vertical
        }

        public FillType fillType;

        private Label _currentHealth;
        private Label _maxHealth;

        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        private VisualElement _healthBarParent;
        private VisualElement _healthBarBackground;
        private VisualElement _healthBarForeground;

        public new class UxmlFactory : UxmlFactory<HealthBar, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlIntAttributeDescription _width = new UxmlIntAttributeDescription() { name = "width", defaultValue = 250 };
            UxmlIntAttributeDescription _height = new UxmlIntAttributeDescription() { name = "height", defaultValue = 15 };
            UxmlFloatAttributeDescription _value = new UxmlFloatAttributeDescription() { name = "value", defaultValue = 1 };
            UxmlEnumAttributeDescription<HealthBar.FillType> _fillType = new UxmlEnumAttributeDescription<FillType>() { name = "fill-type", defaultValue = 0 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                HealthBar healthBarClass = ve as HealthBar;
                healthBarClass.width = _width.GetValueFromBag(bag, cc);
                healthBarClass.height = _height.GetValueFromBag(bag, cc);
                healthBarClass.value = _value.GetValueFromBag(bag, cc);
                healthBarClass.fillType = _fillType.GetValueFromBag(bag, cc);

                healthBarClass.Clear();
                VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UI/HealthBar");
                VisualElement healthBarElement = vt.Instantiate();

                healthBarClass._healthBarParent = healthBarElement.Q<VisualElement>("Character__healthBar");
                healthBarClass._healthBarBackground = healthBarElement.Q<VisualElement>("HP__background");
                healthBarClass._healthBarForeground = healthBarElement.Q<VisualElement>("HP__foreground");
                healthBarClass._currentHealth = healthBarElement.Q<Label>("HP__currentHealth");
                healthBarClass._maxHealth = healthBarElement.Q<Label>("HP__maxHealth");
                healthBarClass.Add(healthBarElement);

                healthBarClass._healthBarParent.style.width = healthBarClass.width;
                healthBarClass._healthBarParent.style.height = healthBarClass.height;
                healthBarClass.style.width = healthBarClass.width;
                healthBarClass.style.height = healthBarClass.height;

                healthBarClass.RegisterValueChangedCallback(healthBarClass.UpdateHealth);
                healthBarClass.FillHealth();
            }
        }

        private void UpdateHealth(ChangeEvent<float> evt)
        {
            FillHealth();
        }

        private void FillHealth()
        {
            if (fillType == FillType.Horizontal)
            {
                _healthBarForeground.style.scale = new Scale(new Vector3(value, 1, 0));
            }
            else
            {
                _healthBarForeground.style.scale = new Scale(new Vector3(1, value, 0));
            }
        }

        public void UpdateHealthDisplay()
        {
            _currentHealth.text = CurrentHealth.ToString();
            _maxHealth.text = MaxHealth.ToString();
        }
    }
}