using System;
using System.Collections.Generic;
using System.Linq;
using Constants;
using Items.ItemInterfaces;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryTabController
    {
        private readonly IReadOnlyList<IItem> _items;
        private readonly InventoryListController _listController;

        public InventoryTabController(VisualElement root, IReadOnlyList<IItem> items)
        {
            _items = items;
            
            MultiColumnListView inventoryListView = root.Q<MultiColumnListView>(InventoryUIConstants.InventoryItems);
            _listController = new InventoryListController(inventoryListView);

            PopulateList();

            GetTabAndSubScribeToSelected(InventoryUIConstants.AllTab, AllTabOnSelected, root);
            GetTabAndSubScribeToSelected(InventoryUIConstants.ToolsAndWeaponsTab, ToolsAndWeaponsTabOnSelected, root);
            GetTabAndSubScribeToSelected(InventoryUIConstants.ClothingTab, ClothingTabOnSelected, root);
            GetTabAndSubScribeToSelected(InventoryUIConstants.ConsumablesTab, ConsumablesTabOnSelected, root);
            GetTabAndSubScribeToSelected(InventoryUIConstants.BooksTab, BooksTabOnSelected, root);
            GetTabAndSubScribeToSelected(InventoryUIConstants.MiscTab, MiscTabOnSelected, root);
        }

        private void GetTabAndSubScribeToSelected(string tabName, Action<Tab> selectedMethod, VisualElement root)
        {
            Tab tab = root.Q<Tab>(tabName);
            tab.selected += selectedMethod;
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

        private void AllTabOnSelected(Tab obj)
        {
            PopulateList();
        }

        private void ToolsAndWeaponsTabOnSelected(Tab obj)
        {
            PopulateListWithCategory(ItemCategory.WeaponsAndTools);
        }
        
        private void ClothingTabOnSelected(Tab obj)
        {
            PopulateListWithCategory(ItemCategory.Clothing);
        }

        private void ConsumablesTabOnSelected(Tab obj)
        {
            PopulateListWithCategory(ItemCategory.Consumables);
        }

        private void BooksTabOnSelected(Tab obj)
        {
            PopulateListWithCategory(ItemCategory.Books);
        }

        private void MiscTabOnSelected(Tab obj)
        {
            PopulateListWithCategory(ItemCategory.Misc);
        }
    }
}