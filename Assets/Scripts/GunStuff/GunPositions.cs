using UnityEngine;

namespace GunStuff
{
    [CreateAssetMenu(fileName = "/Items/GunPositionStateData")]
    public class GunPositions : ScriptableObject
    {
        [SerializeField] private Vector3 avoidingObjectPosition;
        [SerializeField] private Vector3 avoidingObjectRotation;
        [SerializeField] private Vector3 gunDownPosition;
        [SerializeField] private Vector3 gunDownRotation;

        //TODO couch offset?
        
        public Vector3 AvoidingObjectPosition => avoidingObjectPosition;
        public Vector3 AvoidingObjectRotation => avoidingObjectRotation;
        public Vector3 GunDownPosition => gunDownPosition;
        public Vector3 GunDownRotation =>gunDownRotation ;
        // public Vector3 GunUpPosition => gunUpPosition;
        // public Vector3 GunUpRotation => gunUpRotation;
    } 
}