using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace MainMenu
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] UIDocument _uiDoc;

        private VisualElement _root;
        private Button _battleBtn;
        private Button _quitBtn;

        void Awake()
        {
            _root = _uiDoc.rootVisualElement;

            _battleBtn = _root.Q<Button>("Combat__btn");
            _battleBtn.RegisterCallback<ClickEvent>(TestBattle);

            _quitBtn = _root.Q<Button>("Quit__btn");
            _quitBtn.RegisterCallback<ClickEvent>(QuitGame);
        }

        private void TestBattle(ClickEvent evt)
        {
            SceneManager.LoadScene("PrototypeBattle");
        }

        private void QuitGame(ClickEvent evt)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}