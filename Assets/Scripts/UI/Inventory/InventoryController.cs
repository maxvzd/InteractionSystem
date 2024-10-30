using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Items.ItemInterfaces;
using Items.UITemplates;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryController
    {
        private Dictionary<Guid, IItem> _items;
        private readonly InventoryListController _listController;
        private readonly InventoryItemInfoPanelController _itemInfoPanelController;
        private IUIItemModel _selectedItem;
        private InventoryModel _model;

        public EventHandler<ItemEventArgs> RetrieveItemClicked;

        public InventoryController(VisualElement root)
        {
            MultiColumnListView inventoryListView = root.Q<MultiColumnListView>(InventoryUIConstants.InventoryItems);
            _listController = new InventoryListController(inventoryListView);
            _listController.ItemChanged += ListControllerOnItemChanged;

            InventoryTabController tabController = new InventoryTabController(root);
            tabController.TabSelected += TabControllerOnTabSelected;

            _itemInfoPanelController = new InventoryItemInfoPanelController(root);
            _itemInfoPanelController.RetrieveItemButtonClicked += RetrieveItemButtonClicked;

            _items = new Dictionary<Guid, IItem>();
        }

        private void RetrieveItemButtonClicked(object sender, EventArgs e)
        {
            if (_selectedItem.ItemId != Guid.Empty)
            {
                RetrieveItemClicked?.Invoke(this, new ItemEventArgs(_selectedItem.ItemId));
                
                _items.Remove(_selectedItem.ItemId);
                _model.RemoveItem(_selectedItem.ItemId);
                
                _listController.Rebuild(_model);
                _selectedItem = _listController.GetSelectedItem();
                _itemInfoPanelController.SetItem(_selectedItem);
            }
        }

        public void PopulateItems(IReadOnlyDictionary<Guid, IItem> items)
        {
            _items = new Dictionary<Guid, IItem>(items.Count);
            foreach (KeyValuePair<Guid, IItem> item in items)
            {
                _items.Add(item.Key, item.Value);
            }
            PopulateList();
        }

        private void ListControllerOnItemChanged(object sender, ItemChangedEventArgs e)
        {
            _selectedItem = _model.InventoryItems[e.Item.ItemId];
            _itemInfoPanelController.SetItem(_selectedItem);
        }

        private void TabControllerOnTabSelected(object sender, CategoryChangedEventsArgs e)
        {
            if (e.ItemCategory == ItemCategory.None)
            {
                PopulateList();
            }
            else
            {
                PopulateListWithCategory(e.ItemCategory);
            }
        }

        private void PopulateListWithCategory(ItemCategory category)
        {
            _model = ConstructUiModel(_items.Where(x => ItemTypeCategory.GetCategory(x.Value.ItemProperties.Type) == category).Select(x=> x.Value));
            _listController.PopulateInventoryList(_model);
        }

        private void PopulateList()
        {
             _model = ConstructUiModel(_items.Select(x => x.Value));
             _listController.PopulateInventoryList(_model);
        }

        private InventoryModel ConstructUiModel(IEnumerable<IItem> items)
        {
            List<UIItemModel> uiModels = new List<UIItemModel>();
            foreach (IItem item in items)
            {
                uiModels.Add(new UIItemModel(item));
            }
            return new InventoryModel(uiModels);
        }
    }
}