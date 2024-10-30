using System;
using System.Collections.Generic;
using Constants;
using Items;
using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryListController
    {
        private readonly MultiColumnListView _inventoryListView;
        private VisualTreeAsset _inventoryItemTemplate;
        
        public event EventHandler<ItemChangedEventArgs> ItemChanged;

        public InventoryListController(MultiColumnListView listView)
        {
            _inventoryListView = listView;
            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
            foreach (object listItem in obj)
            {
                if (listItem is UIItemModel uiItemModel)
                {
                    ItemChanged?.Invoke(this, new ItemChangedEventArgs(uiItemModel));
                }
            }
        }

        public void PopulateInventoryList(InventoryModel model)
        {
            _inventoryListView.itemsSource = model.ItemList;
            _inventoryListView.columns["Icon"].bindCell = (element, i) =>
            {
                VisualElement iconContainer = element.Q<VisualElement>(InventoryUIConstants.IconElement);
                if (iconContainer is not null)
                {
                    iconContainer.style.backgroundImage = model.ItemList[i].InventoryIcon;
                }
            }; 
            _inventoryListView.columns["Name"].bindCell = (element, i) => SetTextInDisplayLabel(model.ItemList[i].Name, element); 
            _inventoryListView.columns["Category"].bindCell = (element, i) => SetTextInDisplayLabel(model.ItemList[i].Type.ToString(), element); 
            _inventoryListView.columns["Weight"].bindCell = (element, i) => SetTextInDisplayLabel(model.ItemList[i].Weight.ToString("F"), element); 
            _inventoryListView.columns["Volume"].bindCell = (element, i) => SetTextInDisplayLabel(model.ItemList[i].Volume.ToString("F"), element); 
            
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
        
        public void Rebuild(InventoryModel model)
        {
            _inventoryListView.itemsSource = model.ItemList;
            _inventoryListView.Rebuild();
        }

        public IUIItemModel GetSelectedItem()
        {
            if (_inventoryListView.selectedItem is IUIItemModel item)
            {
                return item;
            }
            return new EmptyUIItemModel();
        }
    }
}

public class ItemChangedEventArgs
{
    public UIItemModel Item { get; }
    public ItemChangedEventArgs(UIItemModel item)
    {
        Item = item;
    }
}