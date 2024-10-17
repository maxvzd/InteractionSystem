using UnityEngine;

namespace GunStuff
{
    public class GunPhysicsData : MonoBehaviour
    {
        private Rigidbody _rb;
        private Collider[] _colliders;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _colliders = GetComponentsInChildren<Collider>();
        }

        public void DisablePhysics()
        {
            _rb.isKinematic = true;
            foreach (Collider c in _colliders)
            {
                c.isTrigger = true;
            }
        }
    
        public void EnablePhysics()
        {
            _rb.isKinematic = false;
            foreach (Collider c in _colliders)
            {
                c.isTrigger = false;
            }
        }
    }
}