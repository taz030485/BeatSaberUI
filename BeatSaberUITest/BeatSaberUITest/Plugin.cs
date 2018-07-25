using System;
using System.Collections.Generic;
using System.Linq;
using IllusionPlugin;
using BeatSaberUI;
using UnityEngine.SceneManagement;

namespace BeatSaberUITest
{
    public class Plugin : IPlugin
    {
        public const int MainScene = 1;
        public const int GameScene = 5;

        public string Name => "BeatSaberUITest";
        public string Version => "1.0";

        private bool _init = false;

        private bool testBool;

        public void OnApplicationStart()
        {
            if (_init) return;
            _init = true;
            SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;

            ModMenuUI.AddMenuButton("TestPluginButton", delegate () { Console.WriteLine("TestPluginButton"); });
            ModMenuUI.AddMenuButton("ReloadMenu", delegate () { SceneManager.LoadScene(MainScene); });
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex == MainScene)
            {
                var subMenu = SettingsUI.CreateSubMenu("TestPluginSubMenu"); // Passing in the sub menu label
                var energyBar = subMenu.AddBool("testBool"); // Passing in the option label
                energyBar.GetValue += delegate { return testBool; }; // Delegate returning the bool for display
                energyBar.SetValue += delegate (bool value) { testBool = value; }; // Delegate to set the bool when Apply/Ok is pressed
            }
        }

        public void OnApplicationQuit()
        {

        }

        public void OnLevelWasLoaded(int level)
        {
        }

        public void OnLevelWasInitialized(int level)
        {
        }

        public void OnUpdate()
        {
        }

        public void OnFixedUpdate()
        {
        }
    }
}
