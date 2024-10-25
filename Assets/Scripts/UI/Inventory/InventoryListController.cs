using System.Collections.Generic;
using Items.ItemInterfaces;
using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryListController
    {
        private readonly List<UIItemModel> _inventoryItems = new();
        private VisualTreeAsset _listElementTemplate;
        private ListView _inventoryListView;

        private VisualTreeAsset _inventoryItemTemplate;

        public void InitialiseItemList(VisualElement root, VisualTreeAsset listElementTemplate, IEnumerable<IItem> items)
        {
            foreach (IItem item in items)
            {
                _inventoryItems.Add(new UIItemModel(item));
            }

            _inventoryItemTemplate = listElementTemplate;
            _inventoryListView = root.Q<ListView>("InventoryItems");

            PopulateInventoryList();

            _inventoryListView.selectionChanged += InventoryListViewOnSelectionChanged;
        }

        private void InventoryListViewOnSelectionChanged(IEnumerable<object> obj)
        {
        }

        private void PopulateInventoryList()
        {
            _inventoryListView.makeItem = () =>
            {
                TemplateContainer newItem = _inventoryItemTemplate.Instantiate();
                InventoryItemController newItemController = new InventoryItemController();

                newItem.userData = newItemController;
                newItemController.SetVisualElement(newItem);

                return newItem;
            };

            _inventoryListView.bindItem = (item, index) => { (item.userData as InventoryItemController)?.SetModel(_inventoryItems[index]); };
            _inventoryListView.fixedItemHeight = 100;
            _inventoryListView.itemsSource = _inventoryItems;
        }
    }
}