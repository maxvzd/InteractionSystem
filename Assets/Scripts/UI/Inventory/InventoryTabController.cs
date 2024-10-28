using System;
using Constants;
using UnityEngine.UIElements;

namespace UI.Inventory
{
    public class InventoryTabController
    {
        public event EventHandler<CategoryChangedEventsArgs> TabSelected;
        
        public InventoryTabController(VisualElement root)
        {
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

        private void AllTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.None));
        }

        private void ToolsAndWeaponsTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.WeaponsAndTools));
        }
        
        private void ClothingTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.Clothing));
        }

        private void ConsumablesTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.Consumables));
        }

        private void BooksTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.Books));
        }

        private void MiscTabOnSelected(Tab obj)
        {
            TabSelected?.Invoke(this, new CategoryChangedEventsArgs(ItemCategory.Misc));
        }
    }
}

public class CategoryChangedEventsArgs : EventArgs
{
    public ItemCategory ItemCategory { get; }

    public CategoryChangedEventsArgs(ItemCategory itemCategory)
    {
        ItemCategory = itemCategory;
    }
}