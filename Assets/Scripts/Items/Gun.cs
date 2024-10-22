﻿using System;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public abstract class Gun : MonoBehaviour, IInteractable, IEquipabble, IPhysicsItem
    {
        [SerializeField] private GunProperties gunProperties;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;

        public GunProperties GunProperties => gunProperties;
        public IProperties Properties => gunProperties;
        public bool IsEquipped { get; protected set; }
        
        public abstract void EquipItem(Transform player);
        public abstract void UnEquipItem();

        public void Start()
        {
            _rigidBodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
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