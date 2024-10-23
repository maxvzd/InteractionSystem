using Items;
using Items.ItemInterfaces;
using RootMotion.FinalIK;
using UnityEngine;

// public interface IHeldItem 
// {
//     public Transform Transform { get; }
//     public IItem Item { get; }
//     public OffsetPose OffsetPose { get; }
//     public IEquipabble EquipItem { get; }
//     public bool IsEquippable { get; }
//     public bool HasOffsetPose { get; }
//     public bool HasPhysicsData { get; }
//     public bool IsEmpty { get; }
// }

// public class HeldItem : IHeldItem
// {
//     public HeldItem(Transform itemTransform, IItem item, OffsetPose offsetPose, IEquipabble equipItem)
//     {
//         Transform = itemTransform;
//         Item = item;
//         OffsetPose = offsetPose;
//         EquipItem = equipItem;
//         
//         IsEquippable = equipItem is not null;
//         HasPhysicsData = item is not null;
//         HasOffsetPose = offsetPose is not null;
//     }
//     
//     public Transform Transform { get; }
//     public IItem Item { get; }
//     public OffsetPose OffsetPose { get; }
//     public IEquipabble EquipItem { get; }
//     public bool IsEquippable { get; }
//     public bool HasOffsetPose { get; }
//     public bool HasPhysicsData { get; }
//     public bool IsEmpty => false;
// }

public struct EmptyHand : IItem
{
    public Transform Transform => null;

    public void EnablePhysics()
    {
        //Do nothing
    }

    public void DisablePhysics()
    {
        //Do nothing
    }

    public IItem Item => null;
    public OffsetPose OffsetPose => null;
    public IEquippable EquipItem => null;
    public bool IsEquippable => false;
    public bool HasOffsetPose => false;
    public bool HasPhysicsData => false;
    public bool IsEmpty => true;
}