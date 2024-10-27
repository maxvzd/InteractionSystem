using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public abstract class Gun : BaseItem, IWeapon
    {
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        public GunProperties GunProperties => gunProperties;
        public override IInteractableProperties Properties => gunProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Weapon;
        public EquippedPosition EquippedPosition => equippedPosition;
        public override bool IsEquippable => true;
        public override IItemProperties ItemProperties => GunProperties;
    }
}