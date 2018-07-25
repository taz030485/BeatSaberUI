using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberUI
{
    public class ModMenuUI : MonoBehaviour
    {
        public const int MainScene = 1;
        public const int GameScene = 5;

        public static ModMenuUI Instance;
        public static List<ModMenuButton> modMenuButtons;

        static ModMenuMasterViewController _modMenuMasterViewController;
        static RectTransform _mainMenuRectTransform;
        static MainMenuViewController _mainMenuViewController;

        private Button _buttonInstance;
        private Button _backButtonInstance;
        
        internal static void OnLoad()
        {
            if (Instance != null)
            {
                return;
            }
            new GameObject("ModMenuUI").AddComponent<ModMenuUI>();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void SetupUI()
        {
            try
            {
                _buttonInstance = Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "QuitButton"));
                _backButtonInstance = Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "BackArrowButton"));
                _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                _mainMenuRectTransform = _buttonInstance.transform.parent as RectTransform;
            }
            catch (Exception e)
            {
                Console.WriteLine("Mod Menu UI Awake Exception: " + e);
            }

            CreateMenuButton();
        }

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex == MainScene)
            {
                SetupUI();
            }
        }

        private void CreateMenuButton()
        {

            Button _modMenuButton = CreateUIButton(_mainMenuRectTransform, "SettingsButton");

            try
            {
                (_modMenuButton.transform as RectTransform).anchoredPosition = new Vector2(-37f, 7f);
                (_modMenuButton.transform as RectTransform).sizeDelta = new Vector2(28f, 10f);

                SetButtonText(ref _modMenuButton, "Mods");

                _modMenuButton.onClick.AddListener(delegate () {
                    try
                    {
                        if (_modMenuMasterViewController == null)
                        {
                            _modMenuMasterViewController = CreateViewController<ModMenuMasterViewController>();
                        }
                        _mainMenuViewController.PresentModalViewController(_modMenuMasterViewController, null, false);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Mod Menu UI Button Exception: " + e.Message);
                    }

                });

            }
            catch (Exception e)
            {
                Console.WriteLine("Mod Menu UI Exception: " + e.Message);
            }

        }
        
        public static void PushViewController(VRUIViewController viewController, bool immediately)
        {
            if (Instance == null) return;
            if (_modMenuMasterViewController == null) return;
            _modMenuMasterViewController.PushViewController(viewController, immediately);
        }

        public static void AddMenuButton(string buttonText, Action call)
        {
            if (modMenuButtons == null) modMenuButtons = new List<ModMenuButton>();
            modMenuButtons.Add(new ModMenuButton(buttonText, call));
        }
        
        public Button CreateUIButton(RectTransform parent, string buttonTemplate)
        {
            if (_buttonInstance == null)
            {
                return null;
            }

            Button btn = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == buttonTemplate)), parent, false);
            DestroyImmediate(btn.GetComponent<GameEventOnUIButtonClick>());
            btn.onClick = new Button.ButtonClickedEvent();

            return btn;
        }

        public Button CreateBackButton(RectTransform parent)
        {
            if (_backButtonInstance == null)
            {
                return null;
            }

            Button _button = Instantiate(_backButtonInstance, parent, false);
            DestroyImmediate(_button.GetComponent<GameEventOnUIButtonClick>());
            _button.onClick = new Button.ButtonClickedEvent();

            return _button;
        }

        public T CreateViewController<T>() where T : VRUIViewController
        {
            T vc = new GameObject("CreatedViewController").AddComponent<T>();

            vc.rectTransform.anchorMin = new Vector2(0f, 0f);
            vc.rectTransform.anchorMax = new Vector2(1f, 1f);
            vc.rectTransform.sizeDelta = new Vector2(0f, 0f);
            vc.rectTransform.anchoredPosition = new Vector2(0f, 0f);

            return vc;
        }
        
        public void SetButtonText(ref Button _button, string _text)
        {
            if (_button.GetComponentInChildren<TextMeshProUGUI>() != null)
            {

                _button.GetComponentInChildren<TextMeshProUGUI>().text = _text;
            }

        }
        
        public class ModMenuButton
        {
            public ModMenuButton(string buttonText, Action call)
            {
                this.buttonText = buttonText;
                this.call = call;
            }

            public string buttonText;
            public Action call;
        }
    }
}
