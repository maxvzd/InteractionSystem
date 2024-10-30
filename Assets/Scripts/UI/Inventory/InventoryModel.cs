using System;
using System.Collections.Generic;
using System.Linq;
using Items.UITemplates;

namespace UI.Inventory
{
    public class InventoryModel
    {
        public readonly Dictionary<Guid, UIItemModel> InventoryItems;
        public List<UIItemModel> ItemList => InventoryItems.Values.ToList();

        public InventoryModel(IEnumerable<UIItemModel> items)
        {
            InventoryItems = new Dictionary<Guid, UIItemModel>();
            foreach (UIItemModel model in items)
            {
                InventoryItems.Add(model.ItemId, model);
            }
        }
    }
}