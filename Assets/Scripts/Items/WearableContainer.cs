using Items.ItemSlots;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public class WearableContainer : MonoBehaviour, IInteractable, IWearableContainer, IPhysicsItem
    {
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private WearableContainerProperties wearableContainerProperties;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;
        
        public IProperties Properties => wearableContainerProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Back;
        public EquippedPosition EquippedPosition => equippedPosition;

        private void Start()
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