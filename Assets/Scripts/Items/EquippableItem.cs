using Items.ItemInterfaces;

namespace Items
{
    public abstract class EquippableItem : Item, IEquippable
    {
        public EquipmentSlot EquipmentSlot { get; }
        public EquippedPosition EquippedPosition { get; }
    }
}