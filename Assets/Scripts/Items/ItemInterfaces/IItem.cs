using System;
using Items.UITemplates;
using RootMotion.FinalIK;

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
        IItemProperties ItemProperties { get; }
        IUIItemProperties UIProperties { get; }
        Guid ItemId { get; }
        void RestoreProperties(IItem item);
    }
}