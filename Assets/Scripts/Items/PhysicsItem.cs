using System;
using Items.Properties;
using RootMotion.FinalIK;
using UnityEngine;

namespace Items
{
    public class Item : MonoBehaviour, IInteractable, IPhysicsItem
    {
        [SerializeField] private ItemProperties itemProperties;
        private InteractionObject _interactionObject;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;
        
        public InteractionObject InteractionObject => _interactionObject;
        public ItemProperties ItemProperties => itemProperties;
        public IProperties Properties => itemProperties;

        private void Start()
        {
            _interactionObject = GetComponent<InteractionObject>();
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