using Items.Properties;
using UnityEngine;

namespace Items
{
    public class WearableContainer : MonoBehaviour, IInteractable, IEquipabble, IPhysicsItem
    {
        [SerializeField] private WearableContainerProperties wearableContainerProperties;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;
        
        public IProperties Properties => wearableContainerProperties;
        public bool IsEquipped { get; private set; }

        private void Start()
        {
            _rigidBodies = GetComponentsInChildren<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void EquipItem(Transform player)
        {
            IsEquipped = true;
        }

        public void UnEquipItem()
        {
            IsEquipped = false;
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