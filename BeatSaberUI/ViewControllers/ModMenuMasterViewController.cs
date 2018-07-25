using HMUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberUI
{
    public class ModMenuMasterViewController : VRUINavigationController
    {
        ModMenuUI ui;

        private ModMenuListViewController _modMenuListViewController;

        Button _backButton;

        public int _selectedRow = -1;

        protected override void DidActivate(bool firstActivation, ActivationType activationType)
        {
            ui = ModMenuUI.Instance;

            if (_modMenuListViewController == null)
            {
                _modMenuListViewController = ui.CreateViewController<ModMenuListViewController>();

                PushViewController(_modMenuListViewController, true);
            }
            else
            {
                if (_viewControllers.IndexOf(_modMenuListViewController) < 0)
                {
                    PushViewController(_modMenuListViewController, true);
                }

            }

            if (_backButton == null)
            {
                _backButton = ui.CreateBackButton(rectTransform);

                _backButton.onClick.AddListener(delegate ()
                {

                    DismissModalViewController(null, false);

                });
            }

            base.DidActivate(firstActivation, activationType);

        }

        protected override void DidDeactivate(DeactivationType type)
        {
            base.DidDeactivate(type);
        }
    }
}
