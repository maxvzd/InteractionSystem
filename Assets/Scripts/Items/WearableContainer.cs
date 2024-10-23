using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class WearableContainer : BaseItem, IInteractable, IWearableContainer
    {
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private WearableContainerProperties wearableContainerProperties;
        
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Back;
        public EquippedPosition EquippedPosition => equippedPosition;
        public IProperties Properties => wearableContainerProperties;
        public override bool IsEquippable => true;
    }
}