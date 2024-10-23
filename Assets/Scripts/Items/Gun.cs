using Items.ItemInterfaces;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public abstract class Gun : BaseItem, IInteractable, IWeapon
    {
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        public GunProperties GunProperties => gunProperties;
        public IProperties Properties => gunProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Weapon;
        public EquippedPosition EquippedPosition => equippedPosition;
        public abstract WeaponType WeaponType { get; }
        public override bool IsEquippable => true;
    }
}