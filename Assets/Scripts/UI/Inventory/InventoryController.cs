using System.Collections.Generic;
using System.Linq;
using Constants;
using Items.ItemInterfaces;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryController
    {
        private IEnumerable<IItem> _items;
        private readonly InventoryListController _listController;
        private readonly InventoryItemInfoPanelController _itemInfoPanelController;

        public InventoryController(VisualElement root)
        {
            MultiColumnListView inventoryListView = root.Q<MultiColumnListView>(InventoryUIConstants.InventoryItems);
            _listController = new InventoryListController(inventoryListView);
            _listController.ItemChanged+= ListControllerOnItemChanged;

            InventoryTabController tabController = new InventoryTabController(root);
            tabController.TabSelected += TabControllerOnTabSelected;

            _itemInfoPanelController = new InventoryItemInfoPanelController(root);
        }

        public void PopulateItems(IEnumerable<IItem> items)
        {
            _items = items;
            PopulateList();
        }

        private void ListControllerOnItemChanged(object sender, ItemChangedEventArgs e)
        {
            _itemInfoPanelController.SetItem(e.Item);
        }

        private void TabControllerOnTabSelected(object sender, CategoryChangedEventsArgs e)
        {
            if(e.ItemCategory == ItemCategory.None)
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
            InventoryModel model = new InventoryModel(_items.Where(x => ItemTypeCategory.GetCategory(x.ItemProperties.Type) == category));
            _listController.PopulateInventoryList(model);
        }
        
        private void PopulateList()
        {
            InventoryModel model = new InventoryModel(_items);
            _listController.PopulateInventoryList(model);
        }
    }
}