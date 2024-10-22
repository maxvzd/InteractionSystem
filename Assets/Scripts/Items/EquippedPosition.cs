using UnityEngine;

namespace Items
{
    [CreateAssetMenu]
    public class EquippedPosition : ScriptableObject
    {
        [SerializeField] private Vector3 equippedLocalPosition;
        [SerializeField] private Vector3 equippedLocalRotation;
        
        public Vector3 EquippedLocalPosition => equippedLocalPosition;
        public Vector3 EquippedLocalRotation => equippedLocalRotation;   
    }
}