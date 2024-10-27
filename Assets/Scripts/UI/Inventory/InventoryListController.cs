using System.Collections.Generic;
using Items.ItemInterfaces;
using Items.UITemplates;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryListController
    {
        //private readonly List<UIItemModel> _inventoryItems = new();
        //private VisualTreeAsset _listElementTemplate;
        private MultiColumnListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;
        private InventoryModel _inventoryModel;

        public void InitialiseItemList(VisualElement root, VisualTreeAsset listElementTemplate, IEnumerable<IItem> items)
        {
            _inventoryModel = new InventoryModel();
            foreach (IItem item in items)
            {
                _inventoryModel.AddItem(item);
            }

            //_inventoryItemTemplate = listElementTemplate;
            _inventoryListView = root.Q<MultiColumnListView>("InventoryItems");

            PopulateInventoryList();

            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
        }

        private void PopulateInventoryList()
        {
            _inventoryListView.columns["Icon"].makeCell = () => new VisualElement();
            _inventoryListView.columns["Name"].makeCell = () => new Label();
            _inventoryListView.columns["Category"].makeCell = () => new Label();
            _inventoryListView.columns["Weight"].makeCell = () => new Label();
            _inventoryListView.columns["Volume"].makeCell = () => new Label();

            _inventoryListView.columns["Icon"].bindCell = (element, i) => element.style.backgroundImage = _inventoryModel.InventoryItems[i].InventoryIcon; 
            _inventoryListView.columns["Name"].bindCell = (element, i) => ((Label)element).text = _inventoryModel.InventoryItems[i].Name; 
            _inventoryListView.columns["Category"].bindCell = (element, i) => ((Label)element).text = _inventoryModel.InventoryItems[i].Type.ToString(); 
            _inventoryListView.columns["Weight"].bindCell = (element, i) => ((Label)element).text = _inventoryModel.InventoryItems[i].Weight.ToString("F"); 
            _inventoryListView.columns["Volume"].bindCell = (element, i) => ((Label)element).text = _inventoryModel.InventoryItems[i].Volume.ToString("F"); 
            _inventoryListView.itemsSource = _inventoryModel.InventoryItems;
            
            _inventoryListView.fixedItemHeight = 100;
        }
    }
}