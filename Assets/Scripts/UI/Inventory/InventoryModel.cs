using System.Collections.Generic;
using Items.ItemInterfaces;
using Items.UITemplates;

namespace UI.Inventory
{
    public class InventoryModel
    {
        public readonly List<UIItemModel> InventoryItems = new();

        public void AddItem(IItem item)
        {
            InventoryItems.Add(new UIItemModel(item));
        }
    }
}