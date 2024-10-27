using System.Collections.Generic;
using Constants;
using Items.ItemInterfaces;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryListController
    {
        private MultiColumnListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;
        private InventoryModel _inventoryModel;

        public void InitialiseItemList(VisualElement root, IEnumerable<IItem> items)
        {
            _inventoryModel = new InventoryModel();
            foreach (IItem item in items)
            {
                _inventoryModel.AddItem(item);
            }

            _inventoryListView = root.Q<MultiColumnListView>(InventoryUIConstants.InventoryItems);

            PopulateInventoryList();

            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
        }

        private void PopulateInventoryList()
        {
            _inventoryListView.columns["Icon"].bindCell = (element, i) =>
            {
                VisualElement iconContainer = element.Q<VisualElement>(InventoryUIConstants.IconElement);
                if (iconContainer is not null)
                {
                    iconContainer.style.backgroundImage = _inventoryModel.InventoryItems[i].InventoryIcon;
                }
            }; 
            _inventoryListView.columns["Name"].bindCell = (element, i) => SetTextInDisplayLabel(_inventoryModel.InventoryItems[i].Name, element); 
            _inventoryListView.columns["Category"].bindCell = (element, i) => SetTextInDisplayLabel(_inventoryModel.InventoryItems[i].Type.ToString(), element); 
            _inventoryListView.columns["Weight"].bindCell = (element, i) => SetTextInDisplayLabel(_inventoryModel.InventoryItems[i].Weight.ToString("F"), element); 
            _inventoryListView.columns["Volume"].bindCell = (element, i) => SetTextInDisplayLabel(_inventoryModel.InventoryItems[i].Volume.ToString("F"), element); 
            
            _inventoryListView.itemsSource = _inventoryModel.InventoryItems;
            _inventoryListView.fixedItemHeight = 95;
        }

        private static void SetTextInDisplayLabel(string text, VisualElement root)
        {
            Label label = root.Q<Label>(InventoryUIConstants.DisplayLabel);
            if (label is not null)
            {
                label.text = text;
            }
        }
    }
}