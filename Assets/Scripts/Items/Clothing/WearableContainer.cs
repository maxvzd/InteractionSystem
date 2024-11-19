using System;
using System.Collections.Generic;
using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items.Clothing
{
    public class WearableContainer : BaseItem, IWearableContainer
    {
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Back;
        public EquippedPosition EquippedPosition => equippedPosition;
        public override IInteractableProperties Properties => wearableContainerProperties;
        public override IItemProperties ItemProperties => wearableContainerProperties;
        public override bool IsEquippable => true;
        public IReadOnlyDictionary<Guid, IItem> Inventory => _container.Inventory;
        public float VolumeLimit => _container.VolumeLimit;
        public float WeightLimit => _container.WeightLimit;
        public float CurrentVolume => _container.CurrentVolume;
        public float CurrentWeight => _container.CurrentWeight;

        public Vector3 BackpackOutPositionOffset => backpackOutPositionOffset;
        public Vector3 BackpackOutRotationOffset => backpackOutRotationOffset;
        
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private WearableContainerProperties wearableContainerProperties;
        [SerializeField] private float weightLimit;
        [SerializeField] private float volumeLimit;
        [SerializeField] private Vector3 backpackOutPositionOffset;
        [SerializeField] private Vector3 backpackOutRotationOffset;

        private Container _container;

        public override void RestoreProperties(IItem item)
        {
            //Restore items
        }

        protected override void Awake()
        {
            base.Awake();
            _container = new Container(volumeLimit, weightLimit);
        }
        
        public AddItemToBackpackResult AddItem(IItem itemToAdd) => _container.AddItem(itemToAdd);
        public bool RemoveItem(IItem itemToRemove) =>_container.RemoveItem(itemToRemove);
    }
}