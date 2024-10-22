using Items.ItemSlots;
using Items.Properties;
using UnityEngine;

namespace Items
{
    public abstract class Gun : MonoBehaviour, IInteractable, IWeapon, IPhysicsItem
    {
        [SerializeField] private EquippedPosition equippedPosition;
        [SerializeField] private GunProperties gunProperties;
        private Rigidbody[] _rigidBodies;
        private Collider[] _colliders;

        public GunProperties GunProperties => gunProperties;
        public IProperties Properties => gunProperties;
        public EquipmentSlot EquipmentSlot => EquipmentSlot.Weapon;
        public EquippedPosition EquippedPosition => equippedPosition;
        public abstract WeaponType WeaponType { get; }

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