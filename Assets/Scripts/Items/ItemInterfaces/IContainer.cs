using System;
using System.Collections.Generic;

namespace Items.ItemInterfaces
{
    public interface IContainer
    {
        IReadOnlyDictionary<Guid, IItem> Inventory { get; }
        AddItemToBackpackResult AddItem(IItem itemToAdd);
        bool RemoveItem(IItem itemToRemove);
        float VolumeLimit { get; }
        float WeightLimit { get; }
        float CurrentVolume { get; }
        float CurrentWeight { get; }
    }
}