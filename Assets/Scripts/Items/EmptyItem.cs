using System;
using Constants;
using Items.ItemInterfaces;
using Items.Properties;
using Items.UITemplates;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items
{
    public struct EmptyItem : IItem
    {
        public Transform Transform => null;

        public Guid Id => Guid.Empty;

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
        public IInteractableProperties Properties => new EmptyProperties();
        public IItemProperties ItemProperties => null;
        public IUIItemProperties UIProperties => null;
        public Guid PrefabId => Guid.Empty;
        public Guid ItemId => Guid.Empty;
        public ItemType Type => ItemType.Junk;
    }
}