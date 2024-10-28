using System.Collections.Generic;
using Items.ItemInterfaces;
using Items.UITemplates;

namespace UI.Inventory
{
    public class InventoryModel
    {
        public readonly List<UIItemModel> InventoryItems = new();

        public InventoryModel(IEnumerable<IItem> items)
        {
            foreach (IItem item in items)
            {
                InventoryItems.Add(new UIItemModel(item));
            }
        }
    }
}