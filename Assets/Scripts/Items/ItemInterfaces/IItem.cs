using RootMotion.FinalIK;
using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IItem
    {
        void EnablePhysics();
        void DisablePhysics();
        //public IItem Item { get; }
        public OffsetPose OffsetPose { get; }
        //public IEquipabble EquipItem { get; }
        public bool IsEquippable { get; }
        public bool IsEmpty => false;
        bool HasOffsetPose { get; }
        Transform Transform { get; }
    }
}