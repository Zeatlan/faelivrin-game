using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BattleSystem.UI
{
    public class UIBeginController : MonoBehaviour
    {
        [SerializeField] private UIDocument _uiDocument;

        private VisualElement _root;

        #region screens
        private GroupBox _menu;
        #endregion

        #region Buttons
        private Button _returnBtn;

        private Button _startBtn;
        private Button _organizeUnitsBtn;
        private Button _previewMapBtn;
        private Button _organizeEquipmentsBtn;
        private Button _parametersBtn;
        private Button _quitBtn;
        #endregion

        private bool _isMenuVisible = true;

        [SerializeField] private CameraMovement _camera;

        void Awake()
        {
            _root = _uiDocument.rootVisualElement;

            _menu = _root.Q<GroupBox>("Menu");

            _returnBtn = _root.Q<Button>("ReturnButton");
            _returnBtn.RegisterCallback<ClickEvent>(ToggleMenu);

            _startBtn = _root.Q<Button>("StartBattle");


            _organizeUnitsBtn = _root.Q<Button>("OrganizeUnits");


            _previewMapBtn = _root.Q<Button>("PreviewMap");
            _previewMapBtn.RegisterCallback<ClickEvent>(ToggleMenu);

            _organizeEquipmentsBtn = _root.Q<Button>("OrganizeEquipments");


            _parametersBtn = _root.Q<Button>("Parameters");


            _quitBtn = _root.Q<Button>("Quit");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !_isMenuVisible)
            {
                ToggleMenu(new ClickEvent());
            }
        }

        private void ToggleMenu(ClickEvent evt)
        {
            _isMenuVisible = !_isMenuVisible;

            if (_isMenuVisible)
            {
                _returnBtn.style.display = DisplayStyle.None;
                AnimateMenu(-500f, 0f);
            }
            else
            {
                _menu.style.display = DisplayStyle.Flex;
                AnimateMenu(_menu.style.bottom.value.value, -500f);
            }
        }

        private void AnimateMenu(float startValue, float endValue)
        {
            float currentTime = 0f;
            float totalTime = 0.5f;
            float currentValue = startValue;

            StartCoroutine(AnimateCoroutine());

            IEnumerator AnimateCoroutine()
            {
                while (currentTime < totalTime)
                {
                    float t = currentTime / totalTime;
                    currentValue = Mathf.Lerp(startValue, endValue, t);

                    _menu.style.bottom = currentValue;

                    yield return null;
                    currentTime += Time.deltaTime;
                }

                _menu.style.bottom = endValue;
                _camera.CanMove = !_camera.CanMove;
                PhaseManager.isGamePaused = !PhaseManager.isGamePaused;

                if (!_isMenuVisible)
                {
                    _returnBtn.style.display = DisplayStyle.Flex;
                }
            }

        }
    }
}