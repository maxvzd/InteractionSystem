using UnityEngine;

public class GunPhysicsData : MonoBehaviour
{
        private Rigidbody _rb;
        private Collider[] _colliders;

        public Rigidbody Rigidbody => _rb;
        public Collider[] Colliders => _colliders;
        
        private void Start()
        {
                _rb = GetComponent<Rigidbody>();
                _colliders = GetComponentsInChildren<Collider>();
        }
}