using System;
using System.Collections.Generic;
using Items.ItemInterfaces;
using UI.Inventory;

namespace Items
{
    public class Container : IContainer
    {
        public IReadOnlyDictionary<Guid, IItem> Inventory => _inventory;
        private readonly Dictionary<Guid, IItem> _inventory;
        
        public float VolumeLimit { get; }
        public float WeightLimit { get; }
        public float CurrentVolume { get; }
        public float CurrentWeight { get; }

        public EventHandler<ItemEventArgs> ItemAdded; 
        public EventHandler<ItemEventArgs> ItemRemoved; 

        public Container(float volumeLimit, float weightLimit)
        {
            _inventory = new Dictionary<Guid, IItem>();
            VolumeLimit = volumeLimit;
            WeightLimit = weightLimit;
            CurrentWeight = 0f;
            CurrentVolume = 0f;
        }
        
        public AddItemToBackpackResult AddItem(IItem itemToAdd)
        {
            if (itemToAdd.ItemProperties.Volume + CurrentVolume > VolumeLimit) return AddItemToBackpackResult.TooMuchVolume;
            if (itemToAdd.ItemProperties.Weight + CurrentWeight > WeightLimit) return AddItemToBackpackResult.TooMuchWeight;

            _inventory.Add(itemToAdd.ItemId, itemToAdd);
            ItemAdded?.Invoke(this, new ItemEventArgs(itemToAdd.ItemId));
            return AddItemToBackpackResult.Succeeded;
        }

        public bool RemoveItem(IItem itemToRemove)
        {
            if (!_inventory.Remove(itemToRemove.ItemId)) return false;
            
            ItemRemoved?.Invoke(this, new ItemEventArgs(itemToRemove.ItemId));
            return true;
        }
    }
}