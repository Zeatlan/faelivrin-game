using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleSystem.UI
{
    public class UnitPortrait
    {
        private VisualElement _bg;
        public VisualElement Bg { get => _bg; set => _bg = value; }

        private IMGUIContainer _icon;
        public IMGUIContainer Icon { get => _icon; set => _icon = value; }

        private Label _name;
        public Label Name { get => _name; set => _name = value; }

        public bool IsActive { get; set; }

        public event Action<UnitPortrait> OnClick;

        public UnitPortrait()
        {
            IsActive = false;
            InitializeBackground();
            InitializeIcon();
            InitializeLabel();

            _bg.RegisterCallback<ClickEvent>(evt => OnClick?.Invoke(this));
        }

        private void InitializeBackground()
        {
            _bg = new VisualElement();
            _bg.style.alignItems = Align.Center;
            _bg.style.justifyContent = Justify.Center;
            _bg.style.width = 85;
            _bg.style.height = 125;
            _bg.style.backgroundColor = new Color(0.05f, 0.07f, 0.12f, 1);
            _bg.style.marginRight = 25;

            _bg.style.borderTopColor = new Color(1, 1, 1, 1);
            _bg.style.borderBottomColor = new Color(1, 1, 1, 1);
            _bg.style.borderLeftColor = new Color(1, 1, 1, 1);
            _bg.style.borderRightColor = new Color(1, 1, 1, 1);

            _bg.style.borderTopWidth = 1;
            _bg.style.borderBottomWidth = 1;
            _bg.style.borderLeftWidth = 1;
            _bg.style.borderRightWidth = 1;

            _bg.style.borderTopRightRadius = 5;
            _bg.style.borderTopLeftRadius = 5;
            _bg.style.borderBottomLeftRadius = 5;
            _bg.style.borderBottomRightRadius = 5;

            _bg.style.transitionDuration = new List<TimeValue>() { new TimeValue(300, TimeUnit.Millisecond) };
        }

        private void InitializeIcon()
        {
            _icon = new IMGUIContainer();
            _icon.style.width = 100;
            _icon.style.height = 100;
            _icon.style.position = Position.Absolute;
            _bg.Add(_icon);
        }

        private void InitializeLabel()
        {
            _name = new Label();
            _name.text = "placeholder";
            _name.style.color = new Color(1, 1, 1, 1);
            _name.style.fontSize = 18;
            _name.style.position = Position.Absolute;
            _name.style.bottom = 0;
            _name.style.marginBottom = 15;

            _bg.Add(_name);
        }

        public void SetIcon(StyleBackground icon)
        {
            _icon.style.backgroundImage = icon;
        }

        public void SetName(string name)
        {
            _name.text = name;
        }

        public void Select()
        {
            _bg.style.backgroundColor = new Color(0.08f, 0.22f, 0.09f, 1);
            _bg.style.marginBottom = 25;
            IsActive = true;
        }

        public void Unselect()
        {
            _bg.style.backgroundColor = new Color(0.05f, 0.07f, 0.12f, 1);
            _bg.style.marginBottom = 0;
            IsActive = false;
        }
    }
}
