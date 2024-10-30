using System;
using Items.ItemInterfaces;

namespace UI.Inventory
{
    public class ItemEventArgs : EventArgs
    {
        public Guid ItemID { get; }

        public ItemEventArgs(Guid itemId)
        {
            ItemID = itemId;
        }
    }
}