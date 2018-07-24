using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using VRUI;
using VRUIControls;
using TMPro;
using IllusionPlugin;
using HMUI;
using UnityEngine.Events;
using BeatSaberUI.Utilities;

namespace BeatSaberUI
{
    public class SettingsUI : MonoBehaviour
    {
        public const int MainScene = 1;
        public const int GameScene = 5;
        
        static SettingsUI Instance = null;
        static bool ready = false;
        public static bool Ready
        {
            get => ready;
        }

        static MainMenuViewController _mainMenuViewController = null;
        static SettingsViewController settingsMenu = null;
        static MainSettingsMenuViewController mainSettingsMenu = null;
        static MainSettingsTableView _mainSettingsTableView = null;
        static TableView subMenuTableView = null;
        static MainSettingsTableCell tableCell = null;
        static TableViewHelper subMenuTableViewHelper;

        static Button _pageUpButton = null;
        static Button _pageDownButton = null;
        static Transform othersSubmenu = null;
        static Vector2 buttonOffset = new Vector2(24, 0);

        static SimpleDialogPromptViewController prompt = null;        

        public static void OnLoad()
        {
            if (Instance != null) return;
            new GameObject("Beat Saber UI").AddComponent<SettingsUI>();
        }

        public void Awake()
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

        private void Update()
        {
            //if (Input.GetKeyDown((KeyCode)ConInput.Vive.RightTrackpadPress))
            //{
            //    LogComponents(mainSettingsMenu.transform, "=");
            //}
            //if (Input.GetKeyDown((KeyCode)ConInput.Vive.RightTrackpadPress))
            //{
            //    buttonOffset.x += 0.1f;
            //    Console.WriteLine(buttonOffset.x);
            //}
            //if (Input.GetKeyDown((KeyCode)ConInput.Vive.LeftTrackpadPress))
            //{
            //    buttonOffset.x -= 0.1f;
            //    Console.WriteLine(buttonOffset.x);
            //}
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex == MainScene)
            {
                SetupUI();

                var testSub = CreateSubMenu("Test 1");
                var testSub2 = CreateSubMenu("Test 2");
                var testSub3 = CreateSubMenu("Test 3");
                var testSub4 = CreateSubMenu("Test 4");
                //var testSub5 = CreateSubMenu("Test 5");
                //var testSub6 = CreateSubMenu("Test 6");
            }
        }

        private static void SetupUI()
        {
            if (mainSettingsMenu == null)
            {
                ready = false;
            }

            if (!Ready)
            {
                try
                {
                    var _menuMasterViewController = Resources.FindObjectsOfTypeAll<StandardLevelSelectionFlowCoordinator>().First();
                    prompt = ReflectionUtil.GetPrivateField<SimpleDialogPromptViewController>(_menuMasterViewController, "_simpleDialogPromptViewController");

                    _mainMenuViewController = Resources.FindObjectsOfTypeAll<MainMenuViewController>().First();
                    settingsMenu = Resources.FindObjectsOfTypeAll<SettingsViewController>().FirstOrDefault();
                    mainSettingsMenu = Resources.FindObjectsOfTypeAll<MainSettingsMenuViewController>().FirstOrDefault();
                    _mainSettingsTableView = mainSettingsMenu.GetPrivateField<MainSettingsTableView>("_mainSettingsTableView");
                    subMenuTableView = _mainSettingsTableView.GetComponentInChildren<TableView>();
                    subMenuTableViewHelper = subMenuTableView.gameObject.AddComponent<TableViewHelper>();
                    othersSubmenu = settingsMenu.transform.Find("SubSettingsViewControllers").Find("Others");

                    RectTransform okButton = (RectTransform)settingsMenu.transform.Find("OkButton"); //{x: -17, y: 6}
                    RectTransform CancelButton = (RectTransform)settingsMenu.transform.Find("CancelButton"); // {x: 0, y: 6}
                    RectTransform ApplyButton = (RectTransform)settingsMenu.transform.Find("ApplyButton"); // {x: 17, y: 6}

                    okButton.anchoredPosition += buttonOffset;
                    CancelButton.anchoredPosition += buttonOffset;
                    ApplyButton.anchoredPosition += buttonOffset;

                    if (_mainSettingsTableView != null)
                    {
                        AddPageButtons();
                    }

                    if (tableCell == null)
                    {
                        tableCell = Resources.FindObjectsOfTypeAll<MainSettingsTableCell>().FirstOrDefault();
                        // Get a refence to the Settings Table cell text in case we want to change font size, etc
                        var text = tableCell.GetPrivateField<TextMeshProUGUI>("_settingsSubMenuText");
                    }
                    ready = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Beat Saver UI: Oops - " + e.Message);
                }
            }
        }

        static void AddPageButtons()
        {
            RectTransform viewport = _mainSettingsTableView.GetComponentsInChildren<RectTransform>().First(x => x.name == "Viewport");
            viewport.anchorMin = new Vector2(0f, 0.5f);
            viewport.anchorMax = new Vector2(1f, 0.5f);
            viewport.sizeDelta = new Vector2(0f, 48f);
            viewport.anchoredPosition = new Vector2(0f, 0f);

            if (_pageUpButton == null)
            {
                _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), _mainSettingsTableView.transform, false);
                (_pageUpButton.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
                (_pageUpButton.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
                (_pageUpButton.transform as RectTransform).anchoredPosition = new Vector2(0f, 24f);
                _pageUpButton.interactable = true;
                _pageUpButton.onClick.AddListener(delegate()
                {
                    subMenuTableViewHelper.PageScrollUp();
                });
            }

            if (_pageDownButton == null)
            {
                _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), _mainSettingsTableView.transform, false);
                (_pageDownButton.transform as RectTransform).anchorMin = new Vector2(0.5f, 0.5f);
                (_pageDownButton.transform as RectTransform).anchorMax = new Vector2(0.5f, 0.5f);
                (_pageDownButton.transform as RectTransform).anchoredPosition = new Vector2(0f, -24f);
                _pageDownButton.interactable = true;
                _pageDownButton.onClick.AddListener(delegate ()
                {
                    subMenuTableViewHelper.PageScrollDown();
                });
            }
        }

        public static SubMenu CreateSubMenu(string name)
        {
            return CreateSubMenu<VRUIViewController>(name);
        }

        public static SubMenu CreateSubMenu<T>(string name) where T : VRUIViewController
        {
            if (SceneManager.GetActiveScene().buildIndex != MainScene)
            {
                Console.WriteLine("Cannot create settings menu when no in the main scene.");
                return null;
            }

            SetupUI();

            var subMenuGameObject = Instantiate(othersSubmenu.gameObject, othersSubmenu.transform.parent);
            Transform mainContainer = CleanScreen(subMenuGameObject.transform);

            var oldViewController = subMenuGameObject.GetComponent<VRUIViewController>();
            var newViewController = (T)ReflectionUtil.CopyComponent(oldViewController, typeof(VRUIViewController), typeof(T), subMenuGameObject);
            DestroyImmediate(oldViewController);

            var newSubMenu = new SettingsSubMenuInfo();
            newSubMenu.SetPrivateField("_menuName", name);
            newSubMenu.SetPrivateField("_controller", newViewController);

            var subMenus = mainSettingsMenu.GetPrivateField<SettingsSubMenuInfo[]>("_settingsSubMenuInfos").ToList();
            subMenus.Add(newSubMenu);
            mainSettingsMenu.SetPrivateField("_settingsSubMenuInfos", subMenus.ToArray());

            SubMenu menu = new SubMenu(mainContainer);
            return menu;
        }

        static Transform CleanScreen(Transform screen)
        {
            var container = screen.Find("SettingsContainer");
            var tempList = container.Cast<Transform>().ToList();
            foreach (var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }
            return container;
        }

        public static void LogComponents(Transform t, string prefix)
        {
            Console.WriteLine(prefix + ">" + t.name);

            foreach (var comp in t.GetComponents<MonoBehaviour>())
            {
                Console.WriteLine(prefix + "-->" + comp.GetType());
            }

            foreach (Transform child in t)
            {
                LogComponents(child, prefix + "=");
            }
        }
    }
}
