namespace Items.ItemInterfaces
{
    public interface IEquippable : IItem
    {
        EquipmentSlot EquipmentSlot { get; }
        EquippedPosition EquippedPosition { get; }
    }
}