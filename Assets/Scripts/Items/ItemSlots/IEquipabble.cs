using UnityEngine;

namespace Items.ItemSlots
{
    public interface IEquipabble
    {
        EquipmentSlot EquipmentSlot { get; }
        EquippedPosition EquippedPosition { get; }
    }
}