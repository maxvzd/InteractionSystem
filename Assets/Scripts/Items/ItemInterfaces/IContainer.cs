using System.Collections.Generic;

namespace Items.ItemInterfaces
{
    public interface IContainer
    {
        IReadOnlyList<IItem> Inventory { get; }
        bool AddItem(IItem itemToAdd);
        bool RemoveItem(IItem itemToAdd);
        float VolumeLimit { get; }
        float WeightLimit { get; }
        float CurrentVolume { get; }
        float CurrentWeight { get; }
    }
}