using Items.ItemInterfaces;

namespace Items
{
    public class EquippableItem : Item, IEquippable
    {
        public EquipmentSlot EquipmentSlot { get; }
        public EquippedPosition EquippedPosition { get; }
    }
}