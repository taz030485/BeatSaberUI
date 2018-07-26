using HMUI;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using VRUI;

namespace BeatSaberUI
{
    internal class ModMenuListViewController : VRUIViewController, TableView.IDataSource
    {
        ModMenuMasterViewController _parentMasterViewController;
        ModMenuUI ui;

        public Button _pageUpButton;
        public Button _pageDownButton;

        const int buttonsPerRow = 3;
        readonly Vector2 buttonSize = new Vector2(35, 10);

        static TableView _tableView;
        static ModMenuListTableCell _modListTableCellInstance;
        static TableViewHelper _modMenuTableViewHelper;

        protected override void DidActivate(bool firstActivation, ActivationType type)
        {
            ui = ModMenuUI.Instance;
            _parentMasterViewController = transform.parent.GetComponent<ModMenuMasterViewController>();

            try
            {
                rectTransform.anchorMin = new Vector2(0.1f, 0f);
                rectTransform.anchorMax = new Vector2(0.9f, 1f);

                if (_modListTableCellInstance == null)
                {
                    var modMenuListItem = new GameObject("ModMenuListTableCell");

                    var listItemRectTransform = modMenuListItem.AddComponent<RectTransform>();
                    listItemRectTransform.anchorMin = new Vector2(0f, 0f);
                    listItemRectTransform.anchorMax = new Vector2(1f, 1f);
                    
                    var horiz = modMenuListItem.AddComponent<HorizontalLayoutGroup>();
                    horiz.spacing = 4f;
                    horiz.childControlHeight = false;
                    horiz.childControlWidth = false;
                    horiz.childForceExpandWidth = false;
                    horiz.childAlignment = TextAnchor.MiddleCenter;

                    _modListTableCellInstance = modMenuListItem.AddComponent<ModMenuListTableCell>();

                }

                if (_tableView == null)
                {
                    _tableView = new GameObject().AddComponent<TableView>();

                    _tableView.transform.SetParent(rectTransform, false);

                    _tableView.dataSource = this;

                    (_tableView.transform as RectTransform).anchorMin = new Vector2(0f, 0.5f);
                    (_tableView.transform as RectTransform).anchorMax = new Vector2(1f, 0.5f);
                    (_tableView.transform as RectTransform).sizeDelta = new Vector2(0f, 60f);
                    (_tableView.transform as RectTransform).position = new Vector3(0f, 0f, 2.4f);
                    (_tableView.transform as RectTransform).anchoredPosition = new Vector3(0f, 0f); // -3

                    _modMenuTableViewHelper = _tableView.gameObject.AddComponent<TableViewHelper>();

                }
                else
                {
                    _tableView.ReloadData();
                }

                if (_pageUpButton == null)
                {
                    _pageUpButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageUpButton")), rectTransform, false);
                    (_pageUpButton.transform as RectTransform).anchorMin = new Vector2(0.5f, 1f);
                    (_pageUpButton.transform as RectTransform).anchorMax = new Vector2(0.5f, 1f);
                    (_pageUpButton.transform as RectTransform).anchoredPosition = new Vector2(0f, -10f);//-14
                    _pageUpButton.interactable = true;
                    _pageUpButton.onClick.AddListener(delegate ()
                    {
                        _modMenuTableViewHelper.PageScrollUp();
                    });
                }

                if (_pageDownButton == null)
                {
                    _pageDownButton = Instantiate(Resources.FindObjectsOfTypeAll<Button>().First(x => (x.name == "PageDownButton")), rectTransform, false);
                    (_pageDownButton.transform as RectTransform).anchorMin = new Vector2(0.5f, 0f);
                    (_pageDownButton.transform as RectTransform).anchorMax = new Vector2(0.5f, 0f);
                    (_pageDownButton.transform as RectTransform).anchoredPosition = new Vector2(0f, 10);//8
                    _pageDownButton.interactable = true;
                    _pageDownButton.onClick.AddListener(delegate ()
                    {
                        _modMenuTableViewHelper.PageScrollDown();
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("EXCEPTION IN DidActivate: " + e);
            }
        }

        protected override void DidDeactivate(DeactivationType type)
        {
            base.DidDeactivate(type);
        }

        public void RefreshScreen()
        {
            _tableView.ReloadData();
        }

        public float RowHeight()
        {
            return 12f;
        }

        public int NumberOfRows()
        {
            return (int)Math.Ceiling((float)ModMenuUI.modMenuButtons.Count / buttonsPerRow);
        }

        public TableCell CellForRow(int row)
        {
            ModMenuListTableCell _tableCell = Instantiate(_modListTableCellInstance);
            
            for (int i = 0; i < buttonsPerRow; i++)
            {
                int index = row * buttonsPerRow + i;
                if (ModMenuUI.modMenuButtons.Count > index)
                {
                    var buttonInfo = ModMenuUI.modMenuButtons.ElementAt(index);
                    var button2 = ui.CreateUIButton(_tableCell.transform as RectTransform, "SettingsButton");
                    (button2.transform as RectTransform).sizeDelta = buttonSize;
                    ui.SetButtonText(ref button2, buttonInfo.buttonText);
                    button2.onClick.AddListener(new UnityAction(buttonInfo.call));
                }
            }

            return _tableCell;
        }
    }
}
