﻿using System;
using Items.ItemInterfaces;
using Items.Properties;
using Items.UITemplates;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items
{
    public abstract class BaseItem : MonoBehaviour, IItem
    {
        private InteractionObject _interactionObject;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;

        [SerializeField] private UIItemProperties uiProperties;
        public InteractionObject InteractionObject => _interactionObject;
        public virtual bool IsEquippable => false;
        public OffsetPose OffsetPose { get; private set; }
        public bool HasOffsetPose => OffsetPose is not null;
        public abstract IItemProperties ItemProperties { get; }
        public abstract IInteractableProperties Properties { get; }
        public IUIItemProperties UIProperties => uiProperties;
        public Guid ItemId { get; private set; }
        
        public abstract void RestoreProperties(IItem item);

        protected virtual void Awake()
        {
            _interactionObject = GetComponent<InteractionObject>();
            _rigidBodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
            OffsetPose = GetComponent<OffsetPose>();

            ItemId = Guid.NewGuid();
        }

        public void EnablePhysics()
        {
            PhysicsManager.Enable(_rigidBodies, _colliders);
        }

        public void DisablePhysics()
        {
            PhysicsManager.Disable(_rigidBodies, _colliders);
        }
    }
}