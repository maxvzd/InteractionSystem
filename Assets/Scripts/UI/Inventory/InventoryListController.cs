using System.Collections.Generic;
using Constants;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryListController
    {
        private readonly MultiColumnListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;

        public InventoryListController(MultiColumnListView listView)
        {
            _inventoryListView = listView;
            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
        }

        public void PopulateInventoryList(InventoryModel model)
        {
            _inventoryListView.columns["Icon"].bindCell = (element, i) =>
            {
                VisualElement iconContainer = element.Q<VisualElement>(InventoryUIConstants.IconElement);
                if (iconContainer is not null)
                {
                    iconContainer.style.backgroundImage = model.InventoryItems[i].InventoryIcon;
                }
            }; 
            _inventoryListView.columns["Name"].bindCell = (element, i) => SetTextInDisplayLabel(model.InventoryItems[i].Name, element); 
            _inventoryListView.columns["Category"].bindCell = (element, i) => SetTextInDisplayLabel(model.InventoryItems[i].Type.ToString(), element); 
            _inventoryListView.columns["Weight"].bindCell = (element, i) => SetTextInDisplayLabel(model.InventoryItems[i].Weight.ToString("F"), element); 
            _inventoryListView.columns["Volume"].bindCell = (element, i) => SetTextInDisplayLabel(model.InventoryItems[i].Volume.ToString("F"), element); 
            
            _inventoryListView.itemsSource = model.InventoryItems;
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