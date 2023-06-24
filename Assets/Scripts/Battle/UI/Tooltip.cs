using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Tooltip
{
    private VisualElement _root;

    private bool _isActive;

    #region Elements
    private VisualElement _bg;
    private IMGUIContainer _icon;
    private Label _title;
    private Label _description;
    #endregion

    #region Element settings
    private int _width;
    private int _height;
    private Vector2Int _offset;
    #endregion

    public Tooltip(VisualElement root, int width = 400, int height = 85)
    {
        _isActive = false;

        _root = root;
        _width = width;
        _height = height;
        SetOffset(0, 75);
    }

    public void SetOffset(int x, int y)
    {
        _offset = new Vector2Int(x, y);
    }

    private void SetBackground(Vector2 mousePosition)
    {
        float leftPosition = mousePosition.x - _offset.x;
        float topPosition = mousePosition.y - _offset.y;

        _bg.style.position = Position.Absolute;

        if (leftPosition + _width > Screen.width) leftPosition = mousePosition.x - _width;
        else if (leftPosition - _width < 0) leftPosition = 0;
        _bg.style.left = leftPosition;

        if (topPosition + _height > Screen.height) topPosition = mousePosition.y - _height;
        else if (topPosition - _height < 0) topPosition = 0;
        _bg.style.top = topPosition;

        _bg.style.width = _width;
        _bg.style.height = _height;

        _bg.MarkDirtyRepaint();
    }

    public void ShowTooltip(Vector2 position, string title, string description = "", Sprite icon = null)
    {
        if (_isActive) return;

        VisualTreeAsset vt = Resources.Load<VisualTreeAsset>("UI/Battle/Tooltip");
        _bg = vt.Instantiate();

        SetBackground(position);

        _icon = _bg.Q<IMGUIContainer>("Tooltip__icon");
        _title = _bg.Q<Label>("Main__text");
        _description = _bg.Q<Label>("Secondary__text");

        if (icon == null)
        {
            _icon.style.display = DisplayStyle.None;
        }
        else
        {
            _icon.style.backgroundImage = new StyleBackground(icon);
        }

        if (description == "")
        {
            _description.style.display = DisplayStyle.None;
        }
        else
        {
            _description.text = description;
        }

        _title.text = title;

        _root.Add(_bg);
        _isActive = true;
    }

    /// <summary>
    /// Adapte la taille du tooltip par rapport au texte
    /// </summary>
    public void AutoSizeTooltip()
    {
        _bg.style.width = StyleKeyword.Auto;
        _bg.style.height = StyleKeyword.Auto;
        _width = (int)Mathf.Round(_bg.style.width.value.value);
        _height = (int)Mathf.Round(_bg.style.height.value.value);
    }
    public void HideTooltip()
    {
        if (!_isActive) return;

        _root.Remove(_bg);
        _isActive = false;
    }
}
