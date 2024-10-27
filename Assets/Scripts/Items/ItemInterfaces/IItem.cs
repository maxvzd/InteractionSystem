using Constants;
using Items.UITemplates;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items.ItemInterfaces
{
    public interface IItem : IInteractable
    {
        void EnablePhysics();
        void DisablePhysics();
        OffsetPose OffsetPose { get; }
        bool IsEquippable { get; }
        bool IsEmpty => false;
        bool HasOffsetPose { get; }
        Transform Transform { get; }
        IItemProperties ItemProperties { get; }
        IUIItemProperties UIProperties { get; }
    }
}