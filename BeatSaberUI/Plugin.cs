using System;
using System.Collections.Generic;
using System.Linq;
using IllusionPlugin;

namespace BeatSaberUI
{
    public class Plugin : IPlugin
    {
        public string Name => "SettingsUI";
        public string Version => "1.0";   
    
        private bool _init = false;

        public void OnApplicationStart()
        {
            if (_init) return;
            _init = true;

            SettingsUI.OnLoad();
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
