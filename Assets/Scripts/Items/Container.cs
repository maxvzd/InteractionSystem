using System.Collections.Generic;
using Items.ItemInterfaces;

namespace Items
{
    public class Container : IContainer
    {
        public IReadOnlyList<IItem> Inventory => _inventory;
        
        private readonly List<IItem> _inventory;
        
        public float VolumeLimit { get; }
        public float WeightLimit { get; }
        public float CurrentVolume { get; }
        public float CurrentWeight { get; }

        public Container(float volumeLimit, float weightLimit)
        {
            _inventory = new List<IItem>();
            VolumeLimit = volumeLimit;
            WeightLimit = weightLimit;
            CurrentWeight = 0f;
            CurrentVolume = 0f;
        }
        
        public bool AddItem(IItem itemToAdd)
        {
            if (itemToAdd.ItemProperties.Volume + CurrentVolume > VolumeLimit || itemToAdd.ItemProperties.Weight + CurrentWeight > WeightLimit) return false;
                
            _inventory.Add(itemToAdd);
            return true;
        }

        public bool RemoveItem(IItem itemToAdd)
        {
            throw new System.NotImplementedException();
        }
    }
}