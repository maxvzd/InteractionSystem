using Items.Properties;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IItem : IInteractable
    {
        void EnablePhysics();
        void DisablePhysics();
        public OffsetPose OffsetPose { get; }
        public bool IsEquippable { get; }
        public bool IsEmpty => false;
        bool HasOffsetPose { get; }
        Transform Transform { get; }
        public IItemProperties ItemProperties { get; }
    }
}